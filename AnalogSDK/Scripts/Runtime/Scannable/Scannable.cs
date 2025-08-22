using UnityEngine;

namespace AnalogSDK
{
    public abstract class Scannable : ScriptableObject, IScannable
    {
        [SerializeField] private string barcode;
        public string Barcode
        {
            get => barcode;
            set { barcode = value.ToLower().Replace(" ", ""); }
        }
        public string Title;
    }

    public interface IScannable
    {
        public string Barcode { get; set; }
    }
}
