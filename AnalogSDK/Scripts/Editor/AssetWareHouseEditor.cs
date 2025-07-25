using UnityEditor;
using UnityEngine;
using System.Linq;

namespace AnalogSDK.Editor
{
    public class AssetWareHouseEditor : EditorWindow
    {
        [SerializeField] public static Material Void;
        [SerializeField] public static string SavedMeshesPath = "Assets/Analog SDK/Crates";

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
            allCrates = AssetDatabase.FindAssets("t:Crate")
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

            foreach (var crate in filteredCrates)
            {
                var spawnable = crate as SpawnableCrate;
                if (!spawnable) continue;

                GUILayout.BeginVertical();

                GUILayout.Label(crate.Title, EditorStyles.boldLabel);

                if (GUILayout.Button($"Create {crate.Title} Spawner")) {
                    CreateCrateSpawner(spawnable);
                }
                GUILayout.Label(crate.Description);

                GUILayout.EndVertical();
            }
        }

        private void CreateCrateSpawner(SpawnableCrate selectedCrate)
        {
            GameObject spawnerObject = new GameObject($"{selectedCrate.Title} Spawner");

            CrateSpawner crateSpawner = spawnerObject.AddComponent<CrateSpawner>();

            crateSpawner.selectedCrate = selectedCrate;

            spawnerObject.transform.position = Vector3.zero;

            Debug.Log($"Created CrateSpawner for {selectedCrate.Title}");
        }
    }
}