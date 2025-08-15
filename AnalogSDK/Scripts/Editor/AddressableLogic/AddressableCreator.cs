using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;

namespace AnalogSDK.Editor
{
    public static class AddressableCreator
    {
        //static string selectedPalletPath => AssetDatabase.GetAssetPath(selectedPallet);
        //static string selectedPalletAddress => selectedPallet.Barcode;
        public static AddressableAssetSettings settings => AddressableAssetSettingsDefaultObject.Settings;
        public static AddressableAssetGroup group;
        public static void AddScannableToAddressables(Scannable scannable)
        {
            FindOrCreateGroup();
            var guid = GetGUIDFromObject(scannable);

            var entry = settings.CreateOrMoveEntry(guid, group);
            entry.labels.Add(AssetWarehouse.PalletLabel);
            entry.address = scannable.Barcode;

            //You'll need these to run to save the changes!
            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entry, true);
            AssetDatabase.SaveAssets();
        }

        public static void AddScannableToAddressables(Object Object)
        {
            FindOrCreateGroup();
            var guid = GetGUIDFromObject(Object);

            var entry = settings.CreateOrMoveEntry(guid, group);
            entry.labels.Add(AssetWarehouse.PalletLabel);
            entry.address = Object.name + "";

            //You'll need these to run to save the changes!
            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entry, true);
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

        public static string GetGUIDFromObject(Object Object)
        {
            return AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(Object));
        }

        public static void DeleteGroupFromAddressables(AddressableAssetGroup group)
        {
            settings.RemoveGroup(group);
        }
    }
}
