using UnityEditor;
using UnityEngine;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace AnalogSDK.Editor
{
    public class PallletWindow : EditorWindow
    {
        public string palletTitle = "New Pallet";
        public string palletBarcode = "Barcode";
        public string palletAuthor = "Author";
        public string palletVersion = "1.0";
        public static Pallet selectedPallet;
        [MenuItem("AnalogSDK/Editor/Pallet/Editor")]
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

            palletBarcode = $"{palletAuthor}.{palletTitle}";

            Pallet newPallet = CreateInstance<Pallet>();
            newPallet.Title = palletTitle;
            newPallet.Barcode = palletBarcode;
            newPallet.Author = palletAuthor;
            newPallet.Version = palletVersion;

            AssetDatabase.CreateAsset(newPallet, path + "/" + newPallet.Barcode + ".pallet.asset");
            //AddPalletToAddressables(newPallet);
            AssetDatabase.SaveAssets();
            selectedPallet = newPallet;
        }
        static string selectedPalletPath => AssetDatabase.GetAssetPath(selectedPallet);
        static string selectedPalletAddress => selectedPallet.Barcode;
        public static AddressableAssetSettings settings => AddressableAssetSettingsDefaultObject.Settings;
        public static AddressableAssetGroup group;
        public static void AddPalletToAddressables(Pallet newPallet)
        {
            selectedPallet = newPallet;
            FindOrCreateGroup();

            var guid = AssetDatabase.AssetPathToGUID(selectedPalletPath);

            var entry = settings.CreateOrMoveEntry(guid, group);
            entry.labels.Add(AssetWarehouse.PalletLabel);
            entry.address = selectedPalletAddress;

            //You'll need these to run to save the changes!
            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entry, true);
            selectedPallet = newPallet;
            AssetDatabase.SaveAssets();
        }

        public static void FindOrCreateGroup()
        {
            group = settings.FindGroup(AssetWarehouse.PalletGroup);
            group ??= settings.CreateGroup(AssetWarehouse.PalletGroup, false, false, true, settings.DefaultGroup.Schemas);
            settings.EnableJsonCatalog = true;

            var schema = group.GetSchema<BundledAssetGroupSchema>();
            schema.UseDefaultSchemaSettings = false;
            schema.BundleNaming = BundledAssetGroupSchema.BundleNamingStyle.NoHash;
            schema.IncludeInBuild = true;
            schema.IncludeAddressInCatalog = true;
            schema.IncludeGUIDInCatalog = true;
            schema.IncludeLabelsInCatalog = true;
            schema.BundleMode = BundledAssetGroupSchema.BundlePackingMode.PackSeparately;

            if (!settings.GetLabels().Contains(AssetWarehouse.PalletLabel)) settings.AddLabel(AssetWarehouse.PalletLabel, true);
        }

        public static void RemovePalletFromAddressables(Pallet pallet)
        {
            if (selectedPallet == pallet) return;
            FindOrCreateGroup();

            var guid = AssetDatabase.AssetPathToGUID(selectedPalletPath);

            var entry = settings.RemoveAssetEntry(guid);

            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entry, true);
            AssetDatabase.SaveAssets();
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
}