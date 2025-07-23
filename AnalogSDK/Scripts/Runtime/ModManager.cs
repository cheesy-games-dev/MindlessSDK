using System.Collections.Generic;
using AnalogSDK;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace AnalogSDK
{
    public class ModManager
    {
        public static ModManager Instance { get; private set; }
        public static List<Pallet> Pallets = new();
        public static List<Crate> Crates = new();
        static ModManager()
        {
            if (Instance == null) return;
            Instance = new ModManager();
            Instance.LoadMods();
        }

        public virtual void LoadMods()
        {
            Pallets.Clear();
            Crates.Clear();
            Addressables.LoadAssetsAsync<Pallet>(Addressables.RuntimePath, LoadPallet);
        }

        protected virtual void LoadPallet(Pallet pallet)
        {
            Pallets.Add(pallet);
            Crates.AddRange(pallet.Crates);
            Pallets.Sort();
            Crates.Sort();
        }
    }
}