using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AnalogSDK
{
    public abstract class Crate : Scannable
    {
        public string Description;
        public bool Redacted = false;
        public string[] Tags;
        public Pallet Pallet;
        public Color gizmoColor = Color.white;
        [ContextMenu("Generate Barcode")]
        public void GenerateBarcode()
        {
            Barcode = $"{Pallet.Author}.{Title}";
        }
        public override string ToString()
        {
            return Barcode;
        }
    }

    public abstract class TCrate<T> : Crate where T : Object
    {
        [HideInInspector] public Mesh combinedMesh;
        public ObjectBarcode<T> CrateReference;
    }

    [System.Serializable]
    public struct ObjectBarcode<T> : IScannable where T : Object
    {
        private string _barcode;
        public string Barcode { get => _barcode; set { _barcode = value; } }
        public T Asset => MainAsset.Asset as T;
        public AssetReferenceT<T> MainAsset;
    }

    [System.Serializable]
    public struct CrateBarcode<T> : IScannable where T : Crate
    {
        [SerializeField] private string _barcode;
        public string Barcode { get => _barcode; set { _barcode = value; } }
        public T crate;
        public CrateBarcode(string barcode = "", T crate = null)
        {
            this._barcode = barcode;
            this.crate = crate;
        }
        public CrateBarcode(T crate = null)
        {
            _barcode = crate.Barcode;
            this.crate = crate;
        }
    }
}