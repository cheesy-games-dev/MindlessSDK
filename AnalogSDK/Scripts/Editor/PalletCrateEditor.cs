using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
namespace AnalogSDK.Editor
{
    public class PallletWindow : EditorWindow
    {
        public string palletTitle = "New Pallet";
        public string palletBarcode = "Barcode";
        public string palletAuthor = "Author";
        public string palletVersion = "1.0";
        public static Pallet selectedPallet;
        [MenuItem("AnalogSDK/Editor/Pallet")]
        public static void OpenWindow()
        {
            PallletWindow window = GetWindow<PallletWindow>("Pallet Editor");
            window.Show();
        }
        public void OnGUI()
        {
            EditorGUILayout.Space();
            GUILayout.Label("Pallet Settings", EditorStyles.boldLabel);
            palletTitle = EditorGUILayout.TextField("Pallet Title", palletTitle);
            palletAuthor = EditorGUILayout.TextField("Author", palletAuthor);
            palletVersion = EditorGUILayout.TextField("Version", palletVersion);

            if (GUILayout.Button("Create Pallet"))
            {
                CreatePallet();
            }
        }

        public void CreatePallet()
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

        public void SelectPallet()
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

        public void SelectPalletFromMenu(Pallet pallet)
        {
            selectedPallet = pallet;
        }
    }
    
    public class PalletCrateEditorWindow : EditorWindow
    {
        protected static PallletWindow pallletWindow;
        protected string crateTitle = "New Crate";
        protected string crateBarcode = "emptynullbarcode";
        protected string crateDescription = "Description of the crate.";
        protected List<string> crateTags = new List<string>();
        protected string newTag = "";
        protected Object ObjectReference;

        protected bool isCreatingCrate = false;


        [MenuItem("AnalogSDK/Editor/Spawnable Crates")]
        public static void OpenWindow()
        {
            PalletCrateEditorWindow window = GetWindow<PalletCrateEditorWindow>("Spawnable Crate Editor");
            pallletWindow = new();
            window.Show();
        }

        public virtual void OnGUI()
        {
            pallletWindow ??= new();
            pallletWindow.OnGUI();
            EditorGUILayout.Space();
            GUILayout.Label("Select or Create Crate", EditorStyles.boldLabel);
            if (GUILayout.Button("Select Pallet"))
            {
                pallletWindow.SelectPallet();
            }
            if (PallletWindow.selectedPallet != null)
            {
                GUILayout.Label("Selected Pallet: " + PallletWindow.selectedPallet.Title);

                if (GUILayout.Button("Create Crate"))
                {
                    isCreatingCrate = true;
                }

                if (isCreatingCrate)
                {
                    CreateCrateUI();
                }
            }

            EditorGUILayout.EndScrollView();
        }
        public static bool LevelCrate = false;
        public virtual void CreateCrateUI()
        {
            EditorGUILayout.Space();
            GUILayout.Label("Crate Settings", EditorStyles.boldLabel);
            LevelCrate = GUILayout.Toggle(LevelCrate, "Is Level Crate");
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

            ObjectReference = EditorGUILayout.ObjectField("Select Crate Prefab", ObjectReference, typeof(Object), false);

            if (GUILayout.Button("Save Crate"))
            {
                SaveCrate();
            }

            if (GUILayout.Button("Cancel"))
            {
                isCreatingCrate = false;
            }
        }

        public virtual void SaveCrate()
        {
            if (PallletWindow.selectedPallet == null)
            {
                EditorUtility.DisplayDialog("Error", "Please select or create a pallet first.", "OK");
                return;
            }


            crateBarcode = $"{crateTitle}.{PallletWindow.selectedPallet.Author}.{PallletWindow.selectedPallet.Version}";

            Crate newCrate = LevelCrate ? CreateInstance<LevelCrate>() : CreateInstance<SpawnableCrate>();
            newCrate.Title = crateTitle;
            newCrate.Barcode = crateBarcode;
            newCrate.Description = crateDescription;
            newCrate.Tags = crateTags.ToArray();

            newCrate.CrateObject = ObjectReference;

            string cratePath = "Assets/SDK/pallets/" + PallletWindow.selectedPallet.Title + "_Crates";
            if (!System.IO.Directory.Exists(cratePath))
            {
                System.IO.Directory.CreateDirectory(cratePath);
            }

            AssetDatabase.CreateAsset(newCrate, cratePath + "/" + newCrate.Title + ".asset");
            AssetDatabase.SaveAssets();


            if (PallletWindow.selectedPallet.Crates == null)
            {
                PallletWindow.selectedPallet.Crates = new Crate[] { newCrate };
            }
            else
            {
                List<Crate> crateList = new List<Crate>(PallletWindow.selectedPallet.Crates) { newCrate };
                PallletWindow.selectedPallet.Crates = crateList.ToArray();
            }

            EditorUtility.SetDirty(PallletWindow.selectedPallet);
            AssetDatabase.SaveAssets();

            isCreatingCrate = false;
            EditorUtility.DisplayDialog("Crate Created", $"Crate {newCrate.Title} has been created for pallet {PallletWindow.selectedPallet.Title}.", "OK");
        }
    }
}