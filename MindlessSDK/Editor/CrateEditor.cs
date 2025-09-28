using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MindlessSDK.Editor
{
    [CustomEditor(typeof(Crate), true)]
    [CanEditMultipleObjects]
    public class CrateEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var crate = (Crate)target;
            var spawnable = crate as SpawnableCrate;
            if (spawnable) SpawnableCrateEditor(spawnable);
        }

        private void SpawnableCrateEditor(SpawnableCrate spawnable)
        {
            if (GUILayout.Button("Generate and Save Combined Mesh"))
            {
                RegenerateCombinedMesh();
                SaveMeshToFolder();
            }
            if (GUILayout.Button("Create Spawner"))
            {
                SpawnableCrate.CreateCrateSpawner(spawnable);
            }
        }

        public void RegenerateCombinedMesh()
        {
            foreach (var target in targets)
            {
                var spawnable = target as SpawnableCrate;
                if (!spawnable) return;
                if (spawnable.CrateReference == null)
                {
                    Debug.LogWarning("No crate prefab set.");
                    return;
                }

                spawnable._crateReference.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

                MeshFilter[] meshFilters = (spawnable.CrateReference as GameObject).GetComponentsInChildren<MeshFilter>();
                CombineInstance[] combine = new CombineInstance[meshFilters.Length];

                int i = 0;
                foreach (var meshFilter in meshFilters)
                {
                    combine[i].mesh = meshFilter.sharedMesh;
                    combine[i].transform = meshFilter.transform.localToWorldMatrix;
                    i++;
                }

                spawnable.CombinedMesh = new Mesh();
                spawnable.CombinedMesh.CombineMeshes(combine, true, true);
            }
        }
        public void SaveMeshToFolder()
        {
            foreach (var target in targets)
            {
                var spawnable = target as SpawnableCrate;
                if (!spawnable) return;
                if (spawnable.CombinedMesh == null)
                {
                    Debug.LogWarning("No combined mesh to save.");
                    return;
                }
                string meshPath = $"{EditorAssetWarehouse.preivewsMeshesPath}/{spawnable.Barcode}_Preview.asset";
                Debug.Log(meshPath);

                if (!AssetDatabase.IsValidFolder(EditorAssetWarehouse.preivewsMeshesPath))
                {
                    AssetDatabase.CreateFolder(EditorAssetWarehouse.mainPath, EditorAssetWarehouse.preivewsMeshesKey);
                }

                AssetDatabase.CreateAsset(spawnable.CombinedMesh, meshPath);
                AssetDatabase.SaveAssets();

                Debug.Log($"Mesh saved to: {meshPath}");
            }
        }
    }
}