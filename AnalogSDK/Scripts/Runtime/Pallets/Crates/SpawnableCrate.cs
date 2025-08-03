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
    }
}