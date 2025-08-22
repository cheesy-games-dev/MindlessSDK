using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AnalogSDK.Editor
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
                new GameObject($"Crate Spawner ({spawnable.Barcode})").AddComponent<CrateSpawner>().barcode = new CrateBarcode<SpawnableCrate>(spawnable.Barcode, spawnable);
            }
        }
        public void RegenerateCombinedMesh()
        {
            foreach (var target in targets)
            {
                var spawnable = target as SpawnableCrate;
                if (!spawnable) return;
                if (spawnable.CrateReference.MainAsset == null)
                {
                    Debug.LogWarning("No crate prefab set.");
                    return;
                }

                MeshFilter[] meshFilters = (spawnable.CrateReference.MainAsset.Asset as GameObject).GetComponentsInChildren<MeshFilter>();
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
            foreach (var target in targets)
            {
                var spawnable = target as SpawnableCrate;
                if (!spawnable) return;
                if (spawnable.combinedMesh == null)
                {
                    Debug.LogWarning("No combined mesh to save.");
                    return;
                }
                string previewFolderName = "preview";
                string path = AssetDatabase.GetAssetPath(spawnable.Pallet).Replace(spawnable.Pallet.name+".asset","");
                string preivewPath = Path.Combine(path, previewFolderName);
                string meshPath = $"{preivewPath}/{spawnable.Barcode}_CombinedMesh.asset";

                if (!AssetDatabase.IsValidFolder(preivewPath))
                {
                    AssetDatabase.CreateFolder(path, previewFolderName);
                }

                AssetDatabase.CreateAsset(spawnable.combinedMesh, meshPath);
                AssetDatabase.SaveAssets();

                Debug.Log($"Mesh saved to: {meshPath}");
            }
        }
    }
}