using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AnalogSDK
{
    [CreateAssetMenu(menuName = "AnalogSDK/Spawnable Crate")]
    public class SpawnableCrate : TCrate<Object>
    {
        public virtual async Task<GameObject> SpawnCrate(Vector3 worldPosition, Quaternion worldRotation, Transform parent = null)
        {
            var handle = Addressables.InstantiateAsync(CrateReference);
            await handle.Task;
            GameObject spawned = handle.Result;
            spawned.transform.SetPositionAndRotation(worldPosition, worldRotation);
            spawned.transform.parent = parent;
            return spawned;
        }
        public virtual GameObject SpawnCrate(Transform parent = null)
        {
            return SpawnCrate(parent.position, parent.rotation, parent).Result;
        }
    }
}