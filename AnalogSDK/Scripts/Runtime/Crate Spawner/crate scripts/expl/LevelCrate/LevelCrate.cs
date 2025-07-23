using UnityEngine;

namespace AnalogSDK
{
    public class LevelCrate : ScriptableObject
    {
        public string Barcode = "THISWILLAUTOCREATE";
        public string Title;
        public string Description;
        public bool Redacted = false;
        public string[] Tags;

        public Object LevelScene;
    }
}