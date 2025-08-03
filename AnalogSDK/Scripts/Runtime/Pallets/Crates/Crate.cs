using UnityEngine;

namespace AnalogSDK
{
    public abstract class Crate : ScriptableObject
    {
        public string Barcode = "THISWILLAUTOCREATE";
        public string Name;
        public string Description;
        public bool Redacted = false;
        public string[] Tags;
        public Pallet Pallet;
        [HideInInspector] public Mesh combinedMesh;
        public Object CrateReference;
        public Color gizmoColor = Color.white;
        public override string ToString()
        {
            return Barcode;
        }
    }

    [System.Serializable]
    public struct Barcode<T> where T : Crate
    {
        public string barcode;
        public T crate;
        public Barcode(string barcode = "", T crate = null)
        {
            this.barcode = barcode;
            this.crate = crate;
        }
        public Barcode(T crate = null)
        {
            barcode = crate.Barcode;
            this.crate = crate;
        }
    }
}