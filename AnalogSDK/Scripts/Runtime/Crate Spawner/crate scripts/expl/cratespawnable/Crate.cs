#if UNITY_EDITOR
using AnalogSDK.Editor;
using UnityEditor;
#endif
using UnityEngine;

namespace AnalogSDK
{
    [CreateAssetMenu(menuName = "Crate")]
    public abstract class Crate : ScriptableObject
    {
        public string Barcode = "THISWILLAUTOCREATE";
        public string Title;
        public string Description;
        public bool Redacted = false;
        public string[] Tags;

        public Object CrateObject;

        [HideInInspector] public Mesh combinedMesh;

        public Color gizmoColor = Color.white;
    }

    [CreateAssetMenu(menuName = "AnalogSDK/Spawnable")]
    public class SpawnableCrate : Crate
    {
#if UNITY_EDITOR
        public void RegenerateCombinedMesh()
        {
            if (CrateObject == null)
            {
                Debug.LogWarning("No crate prefab set.");
                return;
            }

            MeshFilter[] meshFilters = (CrateObject as GameObject).GetComponentsInChildren<MeshFilter>();
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];

            int i = 0;
            foreach (var meshFilter in meshFilters)
            {
                combine[i].mesh = meshFilter.sharedMesh;
                combine[i].transform = meshFilter.transform.localToWorldMatrix;
                i++;
            }

            combinedMesh = new Mesh();
            combinedMesh.CombineMeshes(combine, true, true);

        }

        public void SaveMeshToFolder()
        {
            if (combinedMesh == null)
            {
                Debug.LogWarning("No combined mesh to save.");
                return;
            }

            string meshPath = $"{AssetWareHouseEditor.SavedMeshesPath}/{name}_CombinedMesh.asset";

            if (!AssetDatabase.IsValidFolder(AssetWareHouseEditor.SavedMeshesPath))
            {
                AssetDatabase.CreateFolder("Assets/Analog SDK", "Crates");
            }

            AssetDatabase.CreateAsset(combinedMesh, meshPath);
            AssetDatabase.SaveAssets();

            Debug.Log($"Mesh saved to: {meshPath}");
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnCrateSelected()
        {
            if (Selection.activeObject is SpawnableCrate crate)
            {
                crate.RegenerateCombinedMesh();
            }
        }
#endif
    }
}