using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AnalogSDK.Editor
{
    [CustomEditor(typeof(ValidateBehaviour), true)]
    public class ValidateEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var behaviours = targets.Cast<ValidateBehaviour>();
            if (GUILayout.Button("Validate"))
            {
                foreach (var behaviour in behaviours)
                {
                    behaviour.Validate();
                }
            }
        }
    }
}
