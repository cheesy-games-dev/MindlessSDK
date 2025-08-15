using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace AnalogSDK
{
    [CreateAssetMenu(menuName = "AnalogSDK/Spawnable Crate")]
    public class SpawnableCrate : TCrate<Object>
    {
        public virtual async Task<GameObject> SpawnCrate(Vector3 worldPosition, Quaternion worldRotation, Transform parent = null)
        {
            GameObject spawned = Instantiate(CrateReference.Asset) as GameObject;
            spawned.transform.SetPositionAndRotation(worldPosition, worldRotation);
            spawned.transform.parent = parent;
            return spawned;
        }
        public virtual GameObject SpawnCrate(Transform parent)
        {
            return SpawnCrate(parent.position, parent.rotation, parent).Result;
        }
    }
}