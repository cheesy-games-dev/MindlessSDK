using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;
using Object = UnityEngine.Object;
using System.IO;

namespace MindlessSDK.Editor
{
    public class CrateWindow : EditorWindow
    {
        protected static PalletWindow pallletWindow;
        protected string crateTitle = "title";
        protected string crateBarcode = "emptynullbarcode";
        protected string crateDescription = "Description of the crate.";
        protected List<string> crateTags = new List<string>();
        protected string newTag = "";
        protected Object previousObjectReference;
        protected Object ObjectReference;

        protected bool isCreatingCrate = false;


        [MenuItem("MindlessSDK/Editor/Pallet/Crate Editor")]
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
            if (PalletWindow.selectedPallet != null)
            {
                GUILayout.Label("Selected Pallet: " + PalletWindow.selectedPallet.Title);

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
            previousObjectReference = EditorGUILayout.ObjectField("Select Crate Prefab", previousObjectReference, typeof(Object), false);
            if (ObjectReference != previousObjectReference) crateTitle = previousObjectReference.name;
            ObjectReference = previousObjectReference;
            if (PalletWindow.NameError(crateTitle, PalletWindow.titleDummy))
            {
                //EditorGUILayout.HelpBox($"Name cannot be: {crateTitle}", MessageType.Error, true);
                return;
            }
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
            if (PalletWindow.selectedPallet == null)
            {
                EditorUtility.DisplayDialog("Error", "Please select or create a pallet first.", "OK");
                return;
            }

            crateBarcode = $"{PalletWindow.selectedPallet.Barcode}.{crateTitle}";
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
            newCrate.Pallet = PalletWindow.selectedPallet;
            string guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(ObjectReference));
            newCrate.CrateReference.MainAsset = new(guid);
            if (!newCrate.CrateReference.MainAsset.IsValid()) Debug.LogWarning("Make sure there are no errors in the console and the referenced object is marked as an addersable", ObjectReference);
            AssetDatabase.CreateAsset(newCrate, PalletWindow.selectedPalletPath + "/" + newCrate.Barcode + ".crate.asset");
            AssetDatabase.SaveAssets();


            if (PalletWindow.selectedPallet.Crates == null)
            {
                PalletWindow.selectedPallet.Crates = new() { newCrate };
            }
            else
            {
                PalletWindow.selectedPallet.Crates.Add(newCrate);
            }

            EditorUtility.SetDirty(PalletWindow.selectedPallet);
            AssetDatabase.SaveAssets();

            isCreatingCrate = false;
            EditorUtility.DisplayDialog("Crate Created", $"Crate {newCrate.Title} has been created for pallet {PalletWindow.selectedPallet.Title}.", "OK");
            crateTitle = PalletWindow.titleDummy;
            previousObjectReference = null;
            ObjectReference = null;
        }
        
    }
}