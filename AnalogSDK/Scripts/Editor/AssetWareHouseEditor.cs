using UnityEditor;
using UnityEngine;
using System.Linq;

namespace AnalogSDK.Editor
{
    public class AssetWareHouse : EditorWindow
    {
        [SerializeField] public static Material Void;
        [SerializeField] public static string SavedMeshesPath = "Assets/Analog SDK/Crates";

        public static AssetWareHouse Instance;
        private string searchQuery = "";
        private Crate[] allCrates;
        private Crate[] filteredCrates;

        [MenuItem("ANALOG SDK/AssetWareHouse")]
        public static void ShowWindow()
        {
            GetWindow<AssetWareHouse>("AssetWareHouse");
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
                if (GUILayout.Button($"Create {crate.Title} Spawner"))
                {
                    CreateCrateSpawner(crate);
                }

                GUILayout.BeginHorizontal();

                GUILayout.Label(crate.Title, EditorStyles.boldLabel, GUILayout.Width(200));

                GUILayout.Label(crate.Description, GUILayout.Width(300));

                GUILayout.EndHorizontal();
            }
        }

        private void CreateCrateSpawner(Crate selectedCrate)
        {
            GameObject spawnerObject = new GameObject($"{selectedCrate.Title} Spawner");

            CrateSpawner crateSpawner = spawnerObject.AddComponent<CrateSpawner>();

            crateSpawner.selectedCrate = selectedCrate;

            spawnerObject.transform.position = Vector3.zero;

            Debug.Log($"Created CrateSpawner for {selectedCrate.Title}");
        }
    }
}