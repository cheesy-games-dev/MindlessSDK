using UnityEditor;
using UnityEngine;

namespace MindlessSDK.Editor
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
                PalletWindow.selectedPallet = pallet;
                CrateWindow.OpenWindow();
            }
        }
    }
}