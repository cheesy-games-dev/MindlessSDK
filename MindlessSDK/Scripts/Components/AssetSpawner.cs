using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MindlessSDK
{
    public static class AssetSpawner
    {
        public static void Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            Addressables.InstantiateAsync(prefab, position, rotation);
        }
    }
}
