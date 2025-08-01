using UnityEngine;
using System.Collections;
using UltEvents;

namespace AnalogSDK
{
    [RequireComponent(typeof(LevelLoader))]
    [AddComponentMenu("Zones/Zone Items/Zone Level Loader")]
    public class ZoneLevelLoader : ZoneItem
    {
        private LevelLoader LevelLoader;
        public SceneZone.TriggerOption triggerOption;

        public override void Trigger(SceneZone.TriggerOption option)
        {
            if (option == triggerOption)
            {
                LevelLoader = GetComponent<LevelLoader>();
                LevelLoader.LoadLevel();
            }
        }
    }
}