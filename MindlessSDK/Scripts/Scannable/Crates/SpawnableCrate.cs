using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MindlessSDK
{
    public class SpawnableCrate : TCrate<GameObject>
    {
        public static void CreateCrateSpawner(SpawnableCrate spawnable)
        {
            new GameObject($"Crate Spawner ({spawnable.Barcode})").AddComponent<CrateSpawner>().barcode = new CrateBarcode<SpawnableCrate>(spawnable.Barcode, spawnable);
        }
    }
}