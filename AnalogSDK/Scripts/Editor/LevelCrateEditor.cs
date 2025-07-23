using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
namespace AnalogSDK.Editor
{
    public class LevelCrateEditorWindow : EditorWindow
    {
        private string palletTitle = "New Pallet";
        private string palletBarcode = "Barcode";
        private string palletAuthor = "Author";
        private string palletVersion = "1.0";
        private static Pallet selectedPallet;

        private string levelCrateTitle = "New Level Crate";
        private string levelCrateBarcode = "emptynullbarcode";
        private string levelCrateDescription = "Description of the level crate.";
        private List<string> levelCrateTags = new List<string>();
        private string newTag = "";
        private Object levelScene;

        private Vector2 scrollPosition;
        private bool isCreatingLevelCrate = false;

        [MenuItem("ANALOG SDK/Editor/LevelCrates")]
        public static void OpenWindow()
        {
            LevelCrateEditorWindow window = GetWindow<LevelCrateEditorWindow>("Level Crate Editor");
            window.Show();
        }

        private void OnGUI()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            EditorGUILayout.Space();
            GUILayout.Label("Pallet Settings", EditorStyles.boldLabel);
            palletTitle = EditorGUILayout.TextField("Pallet Title", palletTitle);
            palletAuthor = EditorGUILayout.TextField("Author", palletAuthor);
            palletVersion = EditorGUILayout.TextField("Version", palletVersion);

            if (GUILayout.Button("Create Pallet"))
            {
                CreatePallet();
            }

            EditorGUILayout.Space();
            GUILayout.Label("Select or Create Level Crate", EditorStyles.boldLabel);

            if (selectedPallet != null)
            {
                GUILayout.Label("Selected Pallet: " + selectedPallet.Title);

                if (GUILayout.Button("Create Level Crate"))
                {
                    isCreatingLevelCrate = true;
                }

                if (isCreatingLevelCrate)
                {
                    CreateLevelCrateUI();
                }
            }
            else
            {
                if (GUILayout.Button("Select Pallet"))
                {
                    SelectPallet();
                }
            }

            EditorGUILayout.EndScrollView();
        }

        private void CreatePallet()
        {
            string path = "Assets/SDK/pallets";
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }

            palletBarcode = $"{palletTitle}.{palletAuthor}.{palletVersion}";

            Pallet newPallet = ScriptableObject.CreateInstance<Pallet>();
            newPallet.Title = palletTitle;
            newPallet.Barcode = palletBarcode;
            newPallet.Author = palletAuthor;
            newPallet.Version = palletVersion;

            AssetDatabase.CreateAsset(newPallet, path + "/" + palletTitle + ".asset");
            AssetDatabase.SaveAssets();

            selectedPallet = newPallet;
        }

        private void SelectPallet()
        {
            string path = "Assets/SDK/pallets";
            string[] palletGuids = AssetDatabase.FindAssets("t:Pallet", new[] { path });

            GenericMenu menu = new GenericMenu();

            foreach (string guid in palletGuids)
            {
                string palletPath = AssetDatabase.GUIDToAssetPath(guid);
                Pallet pallet = AssetDatabase.LoadAssetAtPath<Pallet>(palletPath);
                menu.AddItem(new GUIContent(pallet.Title), false, () => SelectPalletFromMenu(pallet));
            }

            menu.ShowAsContext();
        }

        private void SelectPalletFromMenu(Pallet pallet)
        {
            selectedPallet = pallet;
        }

        private void CreateLevelCrateUI()
        {
            EditorGUILayout.Space();
            GUILayout.Label("Level Crate Settings", EditorStyles.boldLabel);

            levelCrateTitle = EditorGUILayout.TextField("Level Crate Title", levelCrateTitle);
            levelCrateDescription = EditorGUILayout.TextField("Level Crate Description", levelCrateDescription);

            EditorGUILayout.LabelField("Tags");
            for (int i = 0; i < levelCrateTags.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                levelCrateTags[i] = EditorGUILayout.TextField(levelCrateTags[i]);
                if (GUILayout.Button("Remove", GUILayout.Width(60)))
                {
                    levelCrateTags.RemoveAt(i);
                    return;
                }
                EditorGUILayout.EndHorizontal();
            }

            newTag = EditorGUILayout.TextField("New Tag", newTag);
            if (GUILayout.Button("Add Tag"))
            {
                if (!string.IsNullOrEmpty(newTag))
                {
                    levelCrateTags.Add(newTag);
                    newTag = "";
                }
            }

            levelScene = EditorGUILayout.ObjectField("Select Level Scene", levelScene, typeof(Object), false);


            if (GUILayout.Button("Save Level Crate"))
            {
                SaveLevelCrate();
            }

            if (GUILayout.Button("Cancel"))
            {
                isCreatingLevelCrate = false;
            }
        }

        private void SaveLevelCrate()
        {
            if (selectedPallet == null)
            {
                EditorUtility.DisplayDialog("Error", "Please select or create a pallet first.", "OK");
                return;
            }

            levelCrateBarcode = $"{levelCrateTitle}.{selectedPallet.Author}.{selectedPallet.Version}";

            LevelCrate newLevelCrate = ScriptableObject.CreateInstance<LevelCrate>();
            newLevelCrate.Title = levelCrateTitle;
            newLevelCrate.Barcode = levelCrateBarcode;
            newLevelCrate.Description = levelCrateDescription;
            newLevelCrate.Tags = levelCrateTags.ToArray();
            newLevelCrate.LevelScene = levelScene;

            string levelCratePath = "Assets/SDK/pallets/" + selectedPallet.Title + "_LevelCrates";
            if (!System.IO.Directory.Exists(levelCratePath))
            {
                System.IO.Directory.CreateDirectory(levelCratePath);
            }

            AssetDatabase.CreateAsset(newLevelCrate, levelCratePath + "/" + newLevelCrate.Title + ".asset");
            AssetDatabase.SaveAssets();

            if (selectedPallet.LevelCrates == null)
            {
                selectedPallet.LevelCrates = new LevelCrate[] { newLevelCrate };
            }
            else
            {
                List<LevelCrate> levelCrateList = new List<LevelCrate>(selectedPallet.LevelCrates) { newLevelCrate };
                selectedPallet.LevelCrates = levelCrateList.ToArray();
            }

            EditorUtility.SetDirty(selectedPallet);
            AssetDatabase.SaveAssets();

            isCreatingLevelCrate = false;
            EditorUtility.DisplayDialog("Level Crate Created", $"Level Crate {newLevelCrate.Title} has been created for pallet {selectedPallet.Title}.", "OK");
        }
    }
}