using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MindlessSDK
{
    public abstract class Crate : Scannable
    {
        public string Description;
        public bool Redacted = false;
        public string[] Tags;
        public Pallet Pallet;
        public Color gizmoColor = Color.white;
        public abstract Object CrateReference { get; set; }
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
        public Mesh CombinedMesh;
        public T _crateReference;
        public override Object CrateReference { get => _crateReference; set => _crateReference = value as T; }
    }

    [System.Serializable]
    public struct CrateBarcode<T> : IScannable where T : Crate
    {
        public string _barcode;
        public string Barcode { get => _barcode; set { _barcode = value; } }
        public T _crate;
        public T Crate { get => Crate; set { Crate = value; } }
        public CrateBarcode(string barcode = "", T crate = null)
        {
            _barcode = barcode;
            _crate = crate;
        }
        public CrateBarcode(T crate = null)
        {
            _barcode = crate.Barcode;
            _crate = crate;
        }
    }
}