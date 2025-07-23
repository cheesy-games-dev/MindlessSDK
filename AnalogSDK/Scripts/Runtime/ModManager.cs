using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace AnalogSDK
{
    public class ModManager
    {
        public static ModManager Instance { get; private set; }
        public static List<Pallet> Pallets = new();
        public static List<Crate> Crates = new();
        public static List<LevelCrate> LevelCrates = new();

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
            LevelCrates.Clear();
            Addressables.LoadAssetsAsync<Pallet>(Addressables.RuntimePath, LoadPallet);
        }
        public virtual void LoadLevel(LevelCrate level, LoadSceneMode mode = LoadSceneMode.Single)
        {
            Addressables.LoadSceneAsync(level.LevelScene, mode);
        }

        protected virtual void LoadPallet(Pallet pallet)
        {
            Pallets.Add(pallet);
            Crates.AddRange(pallet.Crates);
            LevelCrates.AddRange(pallet.LevelCrates);
            Pallets.Sort();
            Crates.Sort();
        }
    }
}