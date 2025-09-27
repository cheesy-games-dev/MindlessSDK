using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace MindlessSDK
{
    public class AssetWarehouse : MonoBehaviour
    {
        public List<string> keys;

        public static AssetWarehouse Instance;

        static AssetWarehouse()
        {
            if (!Application.isPlaying || Instance) return;
            new GameObject("AssetWarehouse", typeof(AssetWarehouse));
        }

        public List<Pallet> Pallets = new();
        public List<Crate> Crates = new();
        public List<LevelCrate> LevelCrates = new();
        public List<SpawnableCrate> SpawnableCrates = new();

        protected virtual void Start()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            Instance = this;
            Addressables.LoadAssetsAsync<Pallet>(keys, OnLoadPallet);
            Addressables.LoadAssetsAsync<Crate>(keys, OnLoadCrate);
        }

        protected virtual void OnLoadCrate(Crate crate)
        {
            Crates.Add(crate);
            if(crate is LevelCrate) LevelCrates.Add(crate as LevelCrate);
            if (crate is SpawnableCrate) SpawnableCrates.Add(crate as SpawnableCrate);
        }

        protected virtual void OnLoadPallet(Pallet pallet)
        {
            Pallets.Add(pallet);
        }
    }
}
