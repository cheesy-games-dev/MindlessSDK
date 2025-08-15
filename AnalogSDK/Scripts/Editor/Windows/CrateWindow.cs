using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace AnalogSDK.Editor
{
    public class CrateWindow : EditorWindow
    {
        protected static PallletWindow pallletWindow;
        protected string crateTitle = "New Crate";
        protected string crateBarcode = "emptynullbarcode";
        protected string crateDescription = "Description of the crate.";
        protected List<string> crateTags = new List<string>();
        protected string newTag = "";
        protected Object ObjectReference;

        protected bool isCreatingCrate = false;


        [MenuItem("AnalogSDK/Editor/Pallet/Crate Editor")]
        public static void OpenWindow()
        {
            CrateWindow window = GetWindow<CrateWindow>("Pallet/Crate Editor");
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


            crateBarcode = $"{PallletWindow.selectedPallet.Barcode}.{crateTitle}";

            TCrate<Object> newCrate = LevelCrate ? CreateInstance<LevelCrate>() : CreateInstance<SpawnableCrate>();
            newCrate.Name = crateTitle;
            newCrate.Barcode = crateBarcode;
            newCrate.Description = crateDescription;
            newCrate.Tags = crateTags.ToArray();
            newCrate.Pallet = PallletWindow.selectedPallet;
            newCrate.CrateReference.Asset = ObjectReference;

            string cratePath = "Assets/SDK/pallets/" + PallletWindow.selectedPallet.Title + "_Crates";
            if (!System.IO.Directory.Exists(cratePath))
            {
                System.IO.Directory.CreateDirectory(cratePath);
            }

            AssetDatabase.CreateAsset(newCrate, cratePath + "/" + newCrate.Barcode + ".crate.asset");
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
            EditorUtility.DisplayDialog("Crate Created", $"Crate {newCrate.Name} has been created for pallet {PallletWindow.selectedPallet.Title}.", "OK");
        }
    }
}