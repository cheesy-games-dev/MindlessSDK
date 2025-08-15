using System.Collections;
using AnalogSDK;
using UltEvents;
using UnityEngine;

namespace AnalogSDK
{
    public class LevelLoader : MonoBehaviour, ICrateBarcode
    {
        public Barcode<LevelCrate> LevelCrateToLoad;
        public float delayBeforeLoad = 0f;
        public UltEvent BeforeLoadEvent;
        public UltEvent LoadedEvent;

        public void LoadLevel()
        {
            StartCoroutine(LoadSceneWithDelay());
        }

        private IEnumerator LoadSceneWithDelay()
        {
            if (LoadedEvent != null)
            {
                LoadedEvent.Invoke();
            }

            yield return new WaitForSeconds(delayBeforeLoad);

            AssetWarehouse.Instance.LoadLevel(LevelCrateToLoad.crate);
        }

        public void OnValidate()
        {
            if (LevelCrateToLoad.crate) LevelCrateToLoad.barcode = LevelCrateToLoad.crate.Barcode;
        }
    }
}