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
        public Object CrateReference;
        [HideInInspector] public Mesh combinedMesh;

        public Color gizmoColor = Color.white;
        public override string ToString()
        {
            return Barcode;
        }
    }

    public abstract class CrateT<T> : Crate
    {
        [SerializeField]
        public new T CrateReference;

        public CrateT() { }

        public CrateT(T crateReference)
        {
            CrateReference = crateReference;
        }
    }

    [System.Serializable]
    public class Barcode<T>
    {
        public string barcode;
        public CrateT<T> crate;
        public Barcode(string barcode = "", CrateT<T> crate = null)
        {
            this.barcode = barcode;
            this.crate = crate;
        }
        public Barcode(CrateT<T> crate = null)
        {
            barcode = crate.Barcode;
            this.crate = crate;
        }
        public Barcode()
        {
            barcode = null;
            crate = null;
        }
    }
}