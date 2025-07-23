using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnalogSDK
{
    public class Pallet : ScriptableObject
    {
        public string Barcode;
        public string Title;
        public string Author;
        public string Version;
        public Crate[] Crates;
        public LevelCrate[] LevelCrates;
    }
}