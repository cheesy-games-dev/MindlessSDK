using UltEvents;
using UnityEngine;

namespace AnalogSDK
{
    public class CrateSpawner : MonoBehaviour
    {
        public static List<CrateSpawner> CrateSpawners;
        public Crate selectedCrate;
        public string barcodeInput;
        public bool autoSpawn = true;
        public bool canSpawnOnce = true;
        public UltEvent<GameObject> OnSpawnedCrate;
        public GameObject spawnedCrate;
        public bool canSpawn => ((!spawnedCrate && canSpawnOnce) || (!canSpawnOnce)) && selectedCrate.CrateSpawnable;

        public static UltEvent<CrateSpawner, GameObject> OnCrateSpawnerSpawn;
        private bool gizmoLogged = false;
        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;
        private GameObject combinedMeshObject;

        void Start()
        {
            CrateSpawners.Add(this);
            SearchCrateByBarcode();

            if (autoSpawn && canSpawn)
            {
                SpawnCrate();
            }
        }

        void OnDestroy() {
            CrateSpawners.Remove(this);
        }

        public GameObject SpawnCrate() {
            if (canSpawn) {
                spawnedCrate = Instantiate(selectedCrate.CrateSpawnable, transform.position, transform.rotation);
                OnSpawnedCrate?.Invoke(spawnedCrate);
                OnCrateSpawnerSpawn?.Invoke(this, spawnedCrate);
                Debug.Log($"Spawned crate: {selectedCrate.CrateSpawnable.name}.");
            }
            else {
                if (spawnedCrate && canSpawnOnce)
                    Debug.LogWarning("A crate has already been spawned. Only one crate can be spawned.");
                else
                    Debug.LogWarning("No crate selected or crate prefab is missing.");
            }

            if (canSpawnOnce)
                Destroy(gameObject);

            return spawnedCrate;
        }

        public GameObject DestroyCrate() {
            Destroy(spawnedCrate);
            return spawnedCrate;
        }


        public void SearchCrateByBarcode()
        {
            if (selectedCrate == null && !string.IsNullOrEmpty(barcodeInput))
            {
                if (selectedCrate != null && selectedCrate.Barcode == barcodeInput)
                {
                    Debug.Log($"Selected crate with barcode: {selectedCrate.Barcode}");
                }
                else
                {
                    Debug.LogWarning("Crate with this barcode not found.");
                }
            }
            else if (selectedCrate != null)
            {
                barcodeInput = selectedCrate.Barcode;
            }
        }

        private void VisualizeCombinedMesh()
        {
            if (combinedMeshObject == null)
            {
                combinedMeshObject = new GameObject("CombinedMeshObject");
                combinedMeshObject.transform.SetParent(transform);

                meshFilter = combinedMeshObject.AddComponent<MeshFilter>();
                meshRenderer = combinedMeshObject.AddComponent<MeshRenderer>();
            }

            meshFilter.mesh = selectedCrate.combinedMesh;

            combinedMeshObject.transform.position = transform.position;
            combinedMeshObject.transform.rotation = transform.rotation;
        }

        private void OnDrawGizmos()
        {
            if (selectedCrate != null && selectedCrate.combinedMesh != null)
            {
                Gizmos.color = selectedCrate.gizmoColor;

                Gizmos.DrawMesh(selectedCrate.combinedMesh, transform.position, transform.rotation);

                Gizmos.color = Color.white;
                Bounds meshBounds = selectedCrate.combinedMesh.bounds;
                Gizmos.DrawWireCube(transform.position + meshBounds.center, meshBounds.size);
            }

            if (!gizmoLogged)
            {
                Debug.Log("Gizmo: Showing center of CrateSpawner.");
                gizmoLogged = true;
            }
        }

        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                SearchCrateByBarcode();
            }
        }
    }
}