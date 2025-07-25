using UnityEditor;
using UnityEngine;

namespace AnalogSDK.Editor
{
    [CustomEditor(typeof(Crate), true)]
    public class CrateEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Crate crate = (Crate)target;
            var spawnable = crate as SpawnableCrate;
            if(spawnable)
            if (GUILayout.Button("Generate and Save Combined Mesh"))
            {
                    spawnable.RegenerateCombinedMesh();
                    spawnable.SaveMeshToFolder();
            }
        }
    }
}
