using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MindlessSDK
{
    public class Pallet : Scannable
    {
        public string Author;
        public string Version;
        public List<Crate> Crates;

        [ContextMenu("Generate Barcodes")]
        public void GenerateBarcodes()
        {
            Barcode = $"{Author}.{Title}";
            var crates = Crates.ToList();
            foreach (var crate in crates)
            {
                crate.GenerateBarcode();
            }
            Crates = crates;
        }
        [ContextMenu("Sort Crates")]
        public void SortCrates()
        {
            var crates = Crates.ToList();
            foreach (var crate in crates)
            {
                if (!crate) Crates.Remove(crate);
                if (crates.IndexOf(crate) != crates.LastIndexOf(crate)) Crates.Remove(crate);
            }
            crates = Crates.ToList();
            crates.Sort();
            Crates = crates;
        }
    }
}