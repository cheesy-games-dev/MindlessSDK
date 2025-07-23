using UnityEditor;
using UnityEngine;

namespace AnalogSDK.Editor
{
    [CustomEditor(typeof(Crate))]
    public class CrateEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Crate crate = (Crate)target;

            if (GUILayout.Button("Generate and Save Combined Mesh"))
            {
                crate.RegenerateCombinedMesh();
                crate.SaveMeshToFolder();
            }
        }
    }
}
