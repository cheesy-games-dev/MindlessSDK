using UltEvents;
using UnityEngine;
using System.Collections.Generic;

namespace AnalogSDK {
    public class CrateSpawner : MonoBehaviour, ICrateBarcode {
        public static List<CrateSpawner> CrateSpawners = new();
        public Barcode<SpawnableCrate> barcode;
        public bool manual = false;
        public bool canSpawnOnce = true;
        public UltEvent<CrateSpawner, GameObject> OnSpawnedCrate;
        [HideInInspector] public GameObject spawnedCrate;
        public bool canSpawn => ((!spawnedCrate && canSpawnOnce) || (!canSpawnOnce)) && barcode.crate.CrateReference;
        private bool gizmoLogged = false;
        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;
        private GameObject combinedMeshObject;

        void Start() {
            CrateSpawners.Add(this);
            barcode.crate = AssetWarehouse.GetCrateByBarcode(barcode.barcode) as SpawnableCrate;
            if (!manual && canSpawn) {
                SpawnCrate();
            }
        }

        void OnDestroy() {
            CrateSpawners.Remove(this);
        }

        public GameObject SpawnCrate() {
            if (canSpawn) {
                spawnedCrate = barcode.crate.SpawnCrate(transform.position, transform.rotation);
                OnSpawnedCrate?.Invoke(this, spawnedCrate);
                Debug.Log($"Spawned crate: {barcode.crate.name}.");
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

        private void VisualizeCombinedMesh() {
            if (combinedMeshObject == null) {
                combinedMeshObject = new GameObject("CombinedMeshObject");
                combinedMeshObject.transform.SetParent(transform);

                meshFilter = combinedMeshObject.AddComponent<MeshFilter>();
                meshRenderer = combinedMeshObject.AddComponent<MeshRenderer>();
            }

            meshFilter.mesh = barcode.crate.combinedMesh;

            combinedMeshObject.transform.position = transform.position;
            combinedMeshObject.transform.rotation = transform.rotation;
        }

        private void OnDrawGizmos() {
            if (barcode.crate != null && barcode.crate.combinedMesh != null) {
                Gizmos.color = barcode.crate.gizmoColor;

                Gizmos.DrawMesh(barcode.crate.combinedMesh, transform.position, transform.rotation);

                Gizmos.color = Color.white;
                Bounds meshBounds = barcode.crate.combinedMesh.bounds;
                Gizmos.DrawWireCube(transform.position + meshBounds.center, meshBounds.size);
            }

            if (!gizmoLogged) {
                Debug.Log("Gizmo: Showing center of CrateSpawner.");
                gizmoLogged = true;
            }
        }
        public void OnValidate()
        {
            if (barcode.crate) barcode.barcode = barcode.crate.Barcode;
            gameObject.name = $"Crate Spawner ({barcode.barcode})";
        }
    }
}