using UnityEngine;

namespace AnalogSDK
{
    public abstract class Scannable : ScriptableObject, IScannable
    {
        [SerializeField] private string barcode;
        public string Barcode => barcode;

    }

    public interface IScannable
    {
        public string Barcode { get; }
    }
}
