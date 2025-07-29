using System;
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
            if(spawnable) SpawnableCrateEditor(spawnable);
            
        }

        private void SpawnableCrateEditor(SpawnableCrate spawnable)
        {
            if (GUILayout.Button("Generate and Save Combined Mesh"))
            {
                spawnable.RegenerateCombinedMesh();
                spawnable.SaveMeshToFolder();
            }
            if (GUILayout.Button("Create Spawner"))
            {
                new GameObject($"Crate Spawner ({spawnable.Barcode})").AddComponent<CrateSpawner>().barcode = new(spawnable.Barcode, spawnable);
            }
        }
    }
}
