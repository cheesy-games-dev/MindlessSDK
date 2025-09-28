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
    public static class AssetWarehouseExtensions
    {
        public static Crate GetCrate(this AssetWarehouse warehouse, string barcode)
        {
            return warehouse.Crates.LastOrDefault(c => c.Barcode == barcode);
        }

        public static LevelCrate GetLevelCrate(this AssetWarehouse warehouse, string barcode)
        {
            return warehouse.LevelCrates.LastOrDefault(c => c.Barcode == barcode);
        }

        public static SpawnableCrate GetSpawnableCrate(this AssetWarehouse warehouse, string barcode)
        {
            return warehouse.SpawnableCrates.LastOrDefault(c => c.Barcode == barcode);
        }

        public static Pallet GetPallet(this AssetWarehouse warehouse, string barcode)
        {
            return warehouse.Pallets.LastOrDefault(p => p.Barcode == barcode);
        }
        /*
        static AssetWarehouseExtensions()
        {
            if (!Application.isPlaying || AssetWarehouse.Instance) return;
            new GameObject("AssetWarehouse", typeof(AssetWarehouse));
        }
        */
    }
    public class AssetWarehouse : MonoBehaviour
    {
        public static readonly string ModsPath = $"{Application.persistentDataPath}/Mods";

        public List<string> keys = new();

        public static AssetWarehouse Instance;
        public List<Pallet> Pallets = new();
        public List<Crate> Crates = new();
        public List<LevelCrate> LevelCrates = new();
        public List<SpawnableCrate> SpawnableCrates = new();

        protected virtual void Start()
        {
            DontDestroyOnLoad(gameObject);
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            Addressables.LoadAssetsAsync<Pallet>(keys, OnLoadPallet);
            Addressables.LoadAssetsAsync<Crate>(keys, OnLoadCrate);
        }

        public void Clear()
        {
            Pallets.Clear();
            Crates.Clear();
            LevelCrates.Clear();
            SpawnableCrates.Clear();
        }

        protected virtual void OnLoadCrate(Crate crate)
        {
            Crates.Add(crate);
            if (crate is LevelCrate) LevelCrates.Add(crate as LevelCrate);
            if (crate is SpawnableCrate) SpawnableCrates.Add(crate as SpawnableCrate);
        }

        protected virtual void OnLoadPallet(Pallet pallet)
        {
            Pallets.Add(pallet);
        }
    }
}
