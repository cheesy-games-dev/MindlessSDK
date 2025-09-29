using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MindlessSDK.Editor
{
    [CustomEditor(typeof(ValidateBehaviour), true), CanEditMultipleObjects]
    public class ValidateEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var behaviours = targets.ToList().Cast<ValidateBehaviour>();
            if (GUILayout.Button("Validate"))
            {
                foreach (var behaviour in behaviours)
                {
                    behaviour.Validate();
                }
            }
        }
    }

    [CustomEditor(typeof(MindlessEntity), true), CanEditMultipleObjects]
    public class MindlessEntityEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var entities = targets.ToList().Cast<MindlessEntity>();
            if (GUILayout.Button("Add Poolee"))
            {
                foreach (var entity in entities)
                {
                    entity.gameObject.AddComponent<Poolee>();
                }
            }
            if (GUILayout.Button("Validate"))
            {
                foreach (var entity in entities)
                {
                    entity.Validate();
                }
            }
        }
    }
}
