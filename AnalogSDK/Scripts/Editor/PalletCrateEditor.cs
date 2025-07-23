using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
namespace AnalogSDK.Editor
{
    public class PalletCrateEditorWindow : EditorWindow
    {
        private string palletTitle = "New Pallet";
        private string palletBarcode = "Barcode";
        private string palletAuthor = "Author";
        private string palletVersion = "1.0";
        private static Pallet selectedPallet;

        private string crateTitle = "New Crate";
        private string crateBarcode = "emptynullbarcode";
        private string crateDescription = "Description of the crate.";
        private List<string> crateTags = new List<string>();
        private string newTag = "";
        private GameObject CrateSpawnable;

        private Vector2 scrollPosition;
        private bool isCreatingCrate = false;

        [MenuItem("ANALOG SDK/Editor/SpawnableCrates")]
        public static void OpenWindow()
        {
            PalletCrateEditorWindow window = GetWindow<PalletCrateEditorWindow>("Pallet & Crate Editor");
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
            GUILayout.Label("Select or Create Crate", EditorStyles.boldLabel);

            if (selectedPallet != null)
            {
                GUILayout.Label("Selected Pallet: " + selectedPallet.Title);

                if (GUILayout.Button("Create Crate"))
                {
                    isCreatingCrate = true;
                }

                if (isCreatingCrate)
                {
                    CreateCrateUI();
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

        private void CreateCrateUI()
        {
            EditorGUILayout.Space();
            GUILayout.Label("Crate Settings", EditorStyles.boldLabel);

            crateTitle = EditorGUILayout.TextField("Crate Title", crateTitle);
            crateDescription = EditorGUILayout.TextField("Crate Description", crateDescription);

            EditorGUILayout.LabelField("Tags");
            for (int i = 0; i < crateTags.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                crateTags[i] = EditorGUILayout.TextField(crateTags[i]);
                if (GUILayout.Button("Remove", GUILayout.Width(60)))
                {
                    crateTags.RemoveAt(i);
                    return;
                }
                EditorGUILayout.EndHorizontal();
            }

            newTag = EditorGUILayout.TextField("New Tag", newTag);
            if (GUILayout.Button("Add Tag"))
            {
                if (!string.IsNullOrEmpty(newTag))
                {
                    crateTags.Add(newTag);
                    newTag = "";
                }
            }

            CrateSpawnable = (GameObject)EditorGUILayout.ObjectField("Select Crate Prefab", CrateSpawnable, typeof(GameObject), false);

            if (GUILayout.Button("Save Crate"))
            {
                SaveCrate();
            }

            if (GUILayout.Button("Cancel"))
            {
                isCreatingCrate = false;
            }
        }

        private void SaveCrate()
        {
            if (selectedPallet == null)
            {
                EditorUtility.DisplayDialog("Error", "Please select or create a pallet first.", "OK");
                return;
            }


            crateBarcode = $"{crateTitle}.{selectedPallet.Author}.{selectedPallet.Version}";

            Crate newCrate = ScriptableObject.CreateInstance<Crate>();
            newCrate.Title = crateTitle;
            newCrate.Barcode = crateBarcode;
            newCrate.Description = crateDescription;
            newCrate.Tags = crateTags.ToArray();


            newCrate.CrateSpawnable = CrateSpawnable;

            string cratePath = "Assets/SDK/pallets/" + selectedPallet.Title + "_Crates";
            if (!System.IO.Directory.Exists(cratePath))
            {
                System.IO.Directory.CreateDirectory(cratePath);
            }

            AssetDatabase.CreateAsset(newCrate, cratePath + "/" + newCrate.Title + ".asset");
            AssetDatabase.SaveAssets();


            if (selectedPallet.Crates == null)
            {
                selectedPallet.Crates = new Crate[] { newCrate };
            }
            else
            {
                List<Crate> crateList = new List<Crate>(selectedPallet.Crates) { newCrate };
                selectedPallet.Crates = crateList.ToArray();
            }

            EditorUtility.SetDirty(selectedPallet);
            AssetDatabase.SaveAssets();

            isCreatingCrate = false;
            EditorUtility.DisplayDialog("Crate Created", $"Crate {newCrate.Title} has been created for pallet {selectedPallet.Title}.", "OK");
        }
    }
}