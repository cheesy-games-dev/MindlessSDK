using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnalogSDK
{
    public class Pallet : Scannable
    {
        public string Title;
        public string Author;
        public string Version;
        public Crate[] Crates;
    }
}