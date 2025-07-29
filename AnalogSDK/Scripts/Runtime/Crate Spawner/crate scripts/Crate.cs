#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace AnalogSDK
{
    public abstract class Crate : ScriptableObject
    {
        public string Barcode = "THISWILLAUTOCREATE";
        public string Title;
        public string Description;
        public bool Redacted = false;
        public string[] Tags;

        public Object CrateObject;

        [HideInInspector] public Mesh combinedMesh;

        public Color gizmoColor = Color.white;
    }
    [System.Serializable]
    public struct Barcode
    {
        public string barcode;
        public Crate crate;
        public Barcode(string barcode = "", Crate crate = null)
        {
            this.barcode = barcode;
            this.crate = crate;
        }
        public Barcode(Crate crate = null)
        {
            barcode = "";
            this.crate = crate;
        }
    }
}