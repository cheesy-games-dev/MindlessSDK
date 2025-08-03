using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AnalogSDK.Editor
{
    [CustomEditor(typeof(Pallet), true)]
    public class PalletEditor : UnityEditor.Editor
    {
        public static BuildTarget buildTarget = BuildTarget.StandaloneWindows;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var pallet = (Pallet)target;
            if (GUILayout.Button("Create New Crate"))
            {
                PallletWindow.selectedPallet = pallet;
                CrateWindow.OpenWindow();
            }
            GUILayout.Space(10);
            buildTarget = (BuildTarget)EditorGUILayout.EnumPopup("Build Target", buildTarget);
            if (GUILayout.Button($"Build to {buildTarget}"))
            {
                if(EditorUserBuildSettings.activeBuildTarget != buildTarget) EditorUserBuildSettings.SwitchActiveBuildTarget(0, buildTarget);
                AddressableAssetSettings.CleanPlayerContent();
                PallletWindow.RemovePalletFromAddressables(pallet);
                PallletWindow.AddPalletToAddressables(pallet);
                AddressableAssetSettings.BuildPlayerContent();
            }
        }
    }
}