using UnityEngine;

namespace AnalogSDK
{
    [CreateAssetMenu(menuName = "AnalogSDK/Spawnable Crate")]
    public class SpawnableCrate : Crate
    {
        public new GameObject CrateReference
        {
            get => base.CrateReference as GameObject;
            set => base.CrateReference = value;
        }

        public virtual GameObject SpawnCrate(Vector3 worldPosition, Quaternion worldRotation, Transform parent = null)
        {
            GameObject spawned = Instantiate(CrateReference, worldPosition, worldRotation, parent);
            return spawned;
        }
        public virtual GameObject SpawnCrate(Vector3 worldPosition, Transform parent = null)
        {
            return SpawnCrate(worldPosition, Quaternion.identity, parent);
        }
        public virtual GameObject SpawnCrate(Transform parent = null)
        {
            return SpawnCrate(Vector3.zero, Quaternion.identity, parent);
        }
    }
}