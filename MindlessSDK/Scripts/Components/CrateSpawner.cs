using UltEvents;
using UnityEngine;
using System.Collections.Generic;

namespace MindlessSDK
{
    public class CrateSpawner : MonoBehaviour, ICrateBarcode {
        public static List<CrateSpawner> CrateSpawners = new();
        public CrateBarcode<SpawnableCrate> barcode = new();
        public bool manual = false;
        public bool canSpawnOnce = true;
        public bool parentToSpawner = false;
        public UltEvent<CrateSpawner, GameObject> OnSpawnedCrate;
        [HideInInspector] public GameObject spawnedCrate;
        public bool canSpawn => ((!spawnedCrate && canSpawnOnce) || (!canSpawnOnce)) && barcode.crate.CrateReference;
        private bool gizmoLogged = false;
        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;
        private GameObject combinedMeshObject;

        public virtual void Start()
        {
            Validate();
            CrateSpawners.Add(this);
            if (barcode.crate) barcode.Barcode = barcode.crate.Barcode;
            //AssetWarehouse.TryGetCrateByBarcode(barcode.Barcode, out Crate crate);
            //barcode.crate = crate as SpawnableCrate;
            if (!manual && canSpawn)
            {
                SpawnCrate();
            }
        }

        public virtual void OnDestroy() {
            CrateSpawners.Remove(this);
        }

        public virtual GameObject SpawnCrate()
        {
            if (canSpawn)
            {
                var task = AssetSpawner.Instance.Spawn(barcode.crate, transform.position, transform.rotation, parentToSpawner ? transform : null);
                spawnedCrate = task;
                OnSpawnedCrate?.Invoke(this, spawnedCrate);
                Debug.Log($"Spawned crate: {barcode.crate.name}.");
            }
            else
            {
                if (spawnedCrate && canSpawnOnce)
                    Debug.LogWarning("A crate has already been spawned. Only one crate can be spawned.");
                else
                    Debug.LogWarning("No crate selected or crate prefab is missing.");
            }

            if (canSpawnOnce)
                Destroy(gameObject);

            return spawnedCrate;
        }

        public virtual GameObject DestroyCrate() {
            Destroy(spawnedCrate);
            return spawnedCrate;
        }

        public virtual void VisualizeCombinedMesh() {
            if (combinedMeshObject == null) {
                combinedMeshObject = new GameObject("CombinedMeshObject");
                combinedMeshObject.transform.SetParent(transform);

                meshFilter = combinedMeshObject.AddComponent<MeshFilter>();
                meshRenderer = combinedMeshObject.AddComponent<MeshRenderer>();
            }

            meshFilter.mesh = barcode.crate.CombinedMesh;

            combinedMeshObject.transform.position = transform.position;
            combinedMeshObject.transform.rotation = transform.rotation;
        }

        public virtual void OnDrawGizmos() {
            if (barcode.crate != null && barcode.crate.CombinedMesh != null) {
                Gizmos.color = barcode.crate.gizmoColor;

                Gizmos.DrawMesh(barcode.crate.CombinedMesh, transform.position, transform.rotation);

                Gizmos.color = Color.white;
                Bounds meshBounds = barcode.crate.CombinedMesh.bounds;
                Gizmos.DrawWireCube(transform.position + meshBounds.center, meshBounds.size);
            }

            if (!gizmoLogged) {
                Debug.Log("Gizmo: Showing center of CrateSpawner.");
                gizmoLogged = true;
            }
        }
        public void OnValidate() => Validate();
        public virtual void Validate()
        {
            if (barcode.crate)
                barcode.Barcode = barcode.crate.Barcode;
            gameObject.name = $"Crate Spawner ({barcode.Barcode})";
            AssetWarehouse.Instance.TryGetCrate<Crate>(barcode.Barcode, out var crate);
        }
    }
}