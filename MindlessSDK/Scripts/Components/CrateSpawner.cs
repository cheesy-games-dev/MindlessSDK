using UltEvents;
using UnityEngine;
using System.Collections.Generic;

namespace MindlessSDK
{
    public class CrateSpawner : MonoBehaviour, ICrateBarcode {
        public static List<CrateSpawner> CrateSpawners = new();
        #region Fields
        public CrateBarcode<SpawnableCrate> _barcode = new();
        public bool _manual = false;
        public bool _canSpawnOnce = true;
        public bool _parentToSpawner = false;
        #endregion
        #region Properites
        public CrateBarcode<SpawnableCrate> Barcode
        {
            get => _barcode;
            set
            {
                _barcode = value;
            }
        }
        public bool Manual
        {
            get => _manual;
            set => _manual = value;
        }
        public bool CanSpawnOnce
        {
            get => _canSpawnOnce;
            set => _canSpawnOnce = value;
        }
        public bool ParentToSpawner
        {
            get => _parentToSpawner;
            set => _parentToSpawner = value;
        }
        public bool CanSpawn => ((!spawnedCrate && CanSpawnOnce) || (!CanSpawnOnce)) && Barcode.Crate.CrateReference;
        #endregion


        public UltEvent<CrateSpawner, GameObject> OnSpawnedCrate;
        public GameObject spawnedCrate { get; set; }
        private bool gizmoLogged = false;
        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;
        private GameObject combinedMeshObject;

        public virtual void Start()
        {
            Validate();
            CrateSpawners.Add(this);
            if (Barcode.Crate) _barcode.Barcode = Barcode.Crate.Barcode;
            //AssetWarehouse.TryGetCrateByBarcode(barcode.Barcode, out Crate crate);
            //barcode.crate = crate as SpawnableCrate;
            if (!Manual && CanSpawn)
            {
                SpawnCrate();
            }
        }

        public virtual void OnDestroy() {
            CrateSpawners.Remove(this);
        }

        public virtual GameObject SpawnCrate()
        {
            if (CanSpawn)
            {
                var task = AssetSpawner.Instance.Spawn(Barcode.Crate, transform.position, transform.rotation, ParentToSpawner ? transform : null);
                spawnedCrate = task;
                OnSpawnedCrate?.Invoke(this, spawnedCrate);
                Debug.Log($"Spawned crate: {Barcode.Crate.name}.");
            }
            else
            {
                if (spawnedCrate && CanSpawnOnce)
                    Debug.LogWarning("A crate has already been spawned. Only one crate can be spawned.");
                else
                    Debug.LogWarning("No crate selected or crate prefab is missing.");
            }

            if (CanSpawnOnce)
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

            meshFilter.mesh = Barcode.Crate.CombinedMesh;

            combinedMeshObject.transform.position = transform.position;
            combinedMeshObject.transform.rotation = transform.rotation;
        }
#if UNITY_EDITOR
        public virtual void OnDrawGizmos() {
            if (Barcode.Crate != null && Barcode.Crate.CombinedMesh != null) {
                Gizmos.color = Barcode.Crate.gizmoColor;

                Gizmos.DrawMesh(Barcode.Crate.CombinedMesh, transform.position, transform.rotation);

                Gizmos.color = Color.white;
                Bounds meshBounds = Barcode.Crate.CombinedMesh.bounds;
                Gizmos.DrawWireCube(transform.position + meshBounds.center, meshBounds.size);
            }

            if (!gizmoLogged) {
                Debug.Log("Gizmo: Showing center of CrateSpawner.");
                gizmoLogged = true;
            }
        }
#endif
        public void OnValidate() => Validate();
        public virtual void Validate()
        {
            if (Barcode.Crate)
                _barcode.Barcode = Barcode.Crate.Barcode;
            gameObject.name = $"Crate Spawner ({Barcode.Barcode})";
            AssetWarehouse.Instance.TryGetCrate<Crate>(Barcode.Barcode, out var crate);
        }
    }
}