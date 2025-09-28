using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace MindlessSDK
{
    public static class AssetWarehouseExtensions
    {
        public static bool TryGetCrate<T>(this AssetWarehouse warehouse, string barcode, out T crate) where T : Crate
        {
            crate = GetCrate<T>(warehouse, barcode);
            return crate;
        }
        public static T GetCrate<T>(this AssetWarehouse warehouse, string barcode) where T : Crate
        {
            return (T)warehouse.Crates.LastOrDefault(c => c.Barcode == barcode && c.GetType() == typeof(T));
        }
        public static Pallet TryGetPallet(this AssetWarehouse warehouse, string barcode, out Pallet pallet)
        {
            pallet = GetPallet(warehouse, barcode);
            return pallet;
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

        [Obsolete("Use SceneManager.LoadLevel")]
        public static void LoadLevel(LevelCrate crate)
        {
            SceneManager.Instance.LoadLevel(crate);
        }
    }
}
