using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AnalogSDK
{
    public abstract class Crate : Scannable
    {
        public string Name;
        public string Description;
        public bool Redacted = false;
        public string[] Tags;
        public Pallet Pallet;
        [HideInInspector] public Mesh combinedMesh;
        public Color gizmoColor = Color.white;
        public override string ToString()
        {
            return Barcode;
        }
    }

    public abstract class TCrate<T> : Crate where T : Object
    {
        public AssetReferenceT<T> CrateReference;
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