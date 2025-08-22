using System;
using System.Collections.Generic;
using UnityEngine;

namespace AnalogSDK
{
    public class Pallet : Scannable
    {
        public string Author;
        public string Version;
        public List<Crate> Crates;

        [ContextMenu("Generate Barcode")]
        public void GenerateBarcode()
        {
            Barcode = $"{Author}.{Title}";
        }
        public void GenerateCrateBarcode(Crate crate)
        {
            crate.GenerateBarcode();
        }
        [ContextMenu("Sort Crates")]
        public void SortCrates()
        {
            Crates.Sort();
        }
        [ContextMenu("Fix All Barcodes")]
        public void FixAllCrates()
        {
            GenerateBarcode();
            Crates.ForEach(GenerateCrateBarcode);
        }
        [ContextMenu("Remove Unused Crates")]
        public void RemoveUnusedCrates()
        {
            foreach (var crate in Crates)
            {
                if (!crate) Crates.Remove(crate);
            }
        }
        [ContextMenu("Remove Duplicate Crates")]
        public void RemoveDuplicateCrates()
        {
            foreach (var crate in Crates)
            {
                if (Crates.IndexOf(crate) != Crates.LastIndexOf(crate)) Crates.Remove(crate);
            }
        }
        [ContextMenu("Attempt Fix Pallet")]
        public void FixPallet()
        {
            GenerateBarcode();
            SortCrates();
            FixAllCrates();
            RemoveUnusedCrates();
            RemoveDuplicateCrates();
        }
    }
}