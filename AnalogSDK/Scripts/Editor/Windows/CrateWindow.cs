using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;
using Object = UnityEngine.Object;

namespace AnalogSDK.Editor
{
    public class CrateWindow : EditorWindow
    {
        protected static PallletWindow pallletWindow;
        protected string crateTitle = "title";
        protected string crateBarcode = "emptynullbarcode";
        protected string crateDescription = "Description of the crate.";
        public string cratePath => $"{PallletWindow.path}/{PallletWindow.selectedPallet.Barcode}_Crates";
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
        }
        public enum CrateType
        {
            SPAWNABLE,
            LEVEL,
            AVATAR,
            VFX,
            BGM,
            SFX,
        }
        public static CrateType crateType;
        public virtual void CreateCrateUI()
        {
            EditorGUILayout.Space();
            GUILayout.Label("Crate Settings", EditorStyles.boldLabel);
            crateType = (CrateType)EditorGUILayout.EnumPopup("Crate Type", crateType);
            crateTitle = EditorGUILayout.TextField("Crate Title", crateTitle);
            if (PallletWindow.NameError(crateTitle, PallletWindow.titleDummy))
            {
                //EditorGUILayout.HelpBox($"Name cannot be: {crateTitle}", MessageType.Error, true);
                return;
            }
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
            TCrate<Object> newCrate = null;

            switch (crateType)
            {
                case CrateType.SPAWNABLE:
                    newCrate = CreateInstance<SpawnableCrate>();
                    break;
                case CrateType.LEVEL:
                    newCrate = CreateInstance<LevelCrate>();
                    break;
                case CrateType.AVATAR:
                    //newCrate = CreateInstance<SpawnableCrate>();
                    Debug.LogError("Not Supported in this game");
                    break;
                case CrateType.VFX:
                    //newCrate = CreateInstance<SpawnableCrate>();
                    Debug.LogError("Not Supported in this game");
                    break;
                case CrateType.BGM:
                    //newCrate = CreateInstance<SpawnableCrate>();
                    Debug.LogError("Not Supported in this game");
                    break;
                case CrateType.SFX:
                    //newCrate = CreateInstance<SpawnableCrate>();
                    Debug.LogError("Not Supported in this game");
                    break;
            }
            newCrate.Title = crateTitle;
            newCrate.Barcode = crateBarcode;
            newCrate.Description = crateDescription;
            newCrate.Tags = crateTags.ToArray();
            newCrate.Pallet = PallletWindow.selectedPallet;
            string guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(ObjectReference));
            newCrate.CrateReference.MainAsset = new(guid);
            if (!newCrate.CrateReference.MainAsset.IsValid()) Debug.LogWarning("Make sure there are no errors in the console and the referenced object is marked as an addersable", ObjectReference);

            if (!System.IO.Directory.Exists(cratePath))
            {
                System.IO.Directory.CreateDirectory(cratePath);
            }

            AssetDatabase.CreateAsset(newCrate, cratePath + "/" + newCrate.Barcode + ".crate.asset");
            AssetDatabase.SaveAssets();


            if (PallletWindow.selectedPallet.Crates == null)
            {
                PallletWindow.selectedPallet.Crates = new () { newCrate };
            }
            else
            {
                PallletWindow.selectedPallet.Crates.Add(newCrate);
            }

            EditorUtility.SetDirty(PallletWindow.selectedPallet);
            AssetDatabase.SaveAssets();

            isCreatingCrate = false;
            EditorUtility.DisplayDialog("Crate Created", $"Crate {newCrate.Title} has been created for pallet {PallletWindow.selectedPallet.Title}.", "OK");
            crateTitle = PallletWindow.titleDummy;
        }
        
    }
}