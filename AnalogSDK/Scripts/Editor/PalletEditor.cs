using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AnalogSDK.Editor
{
    [CustomEditor(typeof(Pallet), true)]
    public class PalletEditor : UnityEditor.Editor
    {
        public static AddressablesPlatform addressablesPlatform = AddressablesPlatform.Windows;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var pallet = (Pallet)target;
            if (GUILayout.Button("Create New Crate"))
            {
                PallletWindow.selectedPallet = pallet;
                CrateWindow.OpenWindow();
            }
            addressablesPlatform = (AddressablesPlatform)EditorGUILayout.EnumPopup("Selected Platform", addressablesPlatform);
            if (GUILayout.Button($"Build to {addressablesPlatform}"))
            {
                PallletWindow.AddPalletToAddressables(pallet);
                AssetDatabase.SaveAssets();
                AddressableAssetSettings.CleanPlayerContent();
                AddressableAssetSettings.BuildPlayerContent();
                PallletWindow.RemovePalletFromAddressables(pallet);
                AssetDatabase.SaveAssets();
            }
        }
    }
}