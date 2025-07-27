using UltEvents;
using UnityEngine;
using System.Collections.Generic;

namespace AnalogSDK {
    public class CrateSpawner : MonoBehaviour {
        public static List<CrateSpawner> CrateSpawners = new();
        public Barcode barcode = new();
        public string barcodeInput;
        public bool autoSpawn = true;
        public bool canSpawnOnce = true;
        public UltEvent<GameObject> OnSpawnedCrate;
        public GameObject spawnedCrate;
        public bool canSpawn => ((!spawnedCrate && canSpawnOnce) || (!canSpawnOnce)) && barcode.crate.CrateObject;

        public static UltEvent<CrateSpawner, GameObject> OnCrateSpawnerSpawn;
        private bool gizmoLogged = false;
        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;
        private GameObject combinedMeshObject;

        void Start() {
            CrateSpawners.Add(this);
            AssetWarehouse.GetCrateByBarcode(ref barcode);
            if (autoSpawn && canSpawn) {
                SpawnCrate();
            }
        }

        void OnDestroy() {
            CrateSpawners.Remove(this);
        }

        public GameObject SpawnCrate() {
            if (canSpawn) {
                spawnedCrate = Instantiate(barcode.crate.CrateObject as GameObject, transform.position, transform.rotation);
                OnSpawnedCrate?.Invoke(spawnedCrate);
                OnCrateSpawnerSpawn?.Invoke(this, spawnedCrate);
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

        private void OnValidate() {
            if (!Application.isPlaying) {
                AssetWarehouse.GetCrateByBarcode(ref barcode);
            }
        }
    }
}