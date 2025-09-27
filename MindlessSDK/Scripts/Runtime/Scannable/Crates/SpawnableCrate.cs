using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MindlessSDK
{
    public class SpawnableCrate : TCrate<Object>
    {
        public virtual GameObject SpawnCrate(Vector3 worldPosition, Quaternion worldRotation, Transform parent = null)
        {
            var handle = Addressables.InstantiateAsync(CrateReference.MainAsset);
            GameObject spawned = handle.Result;
            spawned.transform.SetPositionAndRotation(worldPosition, worldRotation);
            spawned.transform.parent = parent;
            return spawned;
        }
        public virtual GameObject SpawnCrate(Transform parent)
        {
            return SpawnCrate(parent.position, parent.rotation, parent);
        }
    }
}