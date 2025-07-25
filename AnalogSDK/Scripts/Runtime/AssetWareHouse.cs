using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace AnalogSDK
{
    public class AssetWareHouse : MonoBehaviour
    {
        public static AssetWareHouse Instance { get; private set; }
        public AssetLabelReference palletLabel = new()
        {
            labelString = "pallet"
        };
        public List<Pallet> Pallets = new();
        public List<SpawnableCrate> SpawnableCrates = new();
        public List<LevelCrate> LevelCrates = new();

        private void Start()
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            LoadMods();
        }

        public virtual void LoadMods()
        {
            Debug.Log("Loading Mods");
            Pallets.Clear();
            SpawnableCrates.Clear();
            LevelCrates.Clear();
            Addressables.LoadAssetsAsync<Pallet>(palletLabel, LoadPallet);
        }
        public virtual void LoadLevel(LevelCrate level, LoadSceneMode mode = LoadSceneMode.Single)
        {
            Addressables.LoadSceneAsync(level.CrateObject, mode);
        }

        protected virtual void LoadPallet(Pallet pallet)
        {
            Debug.Log($"Loaded: {pallet}");
            Debug.Log($"Loaded: {pallet.Crates}");
            Pallets.Add(pallet);
            try
            {
                SpawnableCrates.AddRange(pallet.Crates.Cast<SpawnableCrate>());
            }
            finally
            {
                LevelCrates.AddRange(pallet.Crates.Cast<LevelCrate>());
            }
            Pallets.Sort();
            SpawnableCrates.Sort();
            LevelCrates.Sort();
        }
    }
}