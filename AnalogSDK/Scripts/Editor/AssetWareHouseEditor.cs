using UnityEditor;
using UnityEngine;
using System.Linq;

namespace AnalogSDK.Editor
{
    public class AssetWareHouseEditor : EditorWindow
    {
        public static AssetWareHouseEditor Instance;
        private string searchQuery = "";
        private Crate[] allCrates;
        private Crate[] filteredCrates;

        [MenuItem("AnalogSDK/AssetWareHouse")]
        public static void ShowWindow()
        {
            GetWindow<AssetWareHouseEditor>("AssetWareHouse");
        }

        private void OnEnable()
        {
            Instance = this;
            allCrates = AssetDatabase.FindAssets("t:Crate", new string[]{"Assets", "Assets/SDK"})
                .Select(guid => AssetDatabase.LoadAssetAtPath<Crate>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToArray();

            filteredCrates = allCrates;
        }

        private void OnGUI()
        {
            Instance = this;
            GUILayout.BeginHorizontal();
            GUILayout.Label("Search Crates", GUILayout.Width(150));
            searchQuery = GUILayout.TextField(searchQuery);
            GUILayout.EndHorizontal();

            filteredCrates = string.IsNullOrEmpty(searchQuery)
                ? allCrates
                : allCrates.Where(crate => crate.Title.ToLower().Contains(searchQuery.ToLower())).ToArray();

            GUILayout.Label($"Found {filteredCrates.Length} Crates", EditorStyles.boldLabel);

            foreach (var crate in filteredCrates.Cast<SpawnableCrate>())
            {
                if (crate == null) continue;

                GUILayout.BeginVertical("box");
                GUILayout.Label(crate.Title, EditorStyles.boldLabel);
                GUILayout.Label(crate.Description);
                GUILayout.Label($"Barcode: {crate.Barcode}");
                
                if (GUILayout.Button("Create Crate Spawner"))
                {
                    CreateCrateSpawner(crate);
                }

                if (GUILayout.Button("Regenerate Combined Mesh"))
                {
                    crate.RegenerateCombinedMesh();
                    EditorUtility.SetDirty(crate);
                    AssetDatabase.SaveAssets();
                }

                GUILayout.EndVertical();
            }
        }

        private void CreateCrateSpawner(SpawnableCrate selectedCrate)
        {
            GameObject spawnerObject = new GameObject($"{selectedCrate.Title} Spawner");

            CrateSpawner crateSpawner = spawnerObject.AddComponent<CrateSpawner>();

            crateSpawner.barcode.barcode = selectedCrate.Barcode;
            crateSpawner.barcode.crate = selectedCrate;

            spawnerObject.transform.position = Vector3.zero;

            Debug.Log($"Created CrateSpawner for {selectedCrate.Title}");
        }
    }
}