using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AnalogSDK.Editor
{
    [CustomEditor(typeof(Crate), true)]
    [CanEditMultipleObjects]
    public class CrateEditor : UnityEditor.Editor
    {
        public static Crate[] selectedCrates { get; private set; }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var crate = (Crate)target;
            selectedCrates = (Crate[])targets;
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
                new GameObject($"Crate Spawner ({spawnable.Barcode})").AddComponent<CrateSpawner>().barcode = new Barcode<SpawnableCrate>(spawnable.Barcode, spawnable);
            }
        }
        public void RegenerateCombinedMesh()
        {
            foreach (var crate in selectedCrates)
            {
                var spawnable = crate as SpawnableCrate;
                if (!spawnable) return;
                if (spawnable.CrateReference == null)
                {
                    Debug.LogWarning("No crate prefab set.");
                    return;
                }

                MeshFilter[] meshFilters = spawnable.CrateReference.GetComponentsInChildren<MeshFilter>();
                CombineInstance[] combine = new CombineInstance[meshFilters.Length];

                int i = 0;
                foreach (var meshFilter in meshFilters)
                {
                    combine[i].mesh = meshFilter.sharedMesh;
                    combine[i].transform = meshFilter.transform.localToWorldMatrix;
                    i++;
                }

                spawnable.combinedMesh = new Mesh();
                spawnable.combinedMesh.CombineMeshes(combine, true, true);
            }
        }

        public void SaveMeshToFolder()
        {
            foreach (var crate in selectedCrates)
            {
                var spawnable = crate as SpawnableCrate;
                if (!spawnable) return;
                if (spawnable.combinedMesh == null)
                {
                    Debug.LogWarning("No combined mesh to save.");
                    return;
                }

                string meshPath = $"{AssetWarehouse.SavedMeshesPath}/{spawnable.Name}_CombinedMesh.asset";

                if (!AssetDatabase.IsValidFolder(AssetWarehouse.SavedMeshesPath))
                {
                    AssetDatabase.CreateFolder("Assets/SDK", "meshes");
                }

                AssetDatabase.CreateAsset(spawnable.combinedMesh, meshPath);
                AssetDatabase.SaveAssets();

                Debug.Log($"Mesh saved to: {meshPath}");
            }
        }
    }
}
