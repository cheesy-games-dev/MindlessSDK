using System.Collections;
using AnalogSDK;
using UltEvents;
using UnityEngine;

namespace AnalogSDK
{
    public class LevelLoader : MonoBehaviour, ICrateBarcode
    {
        public CrateBarcode<LevelCrate> LevelCrateToLoad;
        public float delayBeforeLoad = 0f;
        public UltEvent BeforeLoadEvent;
        public UltEvent LoadedEvent;

        public void LoadLevel()
        {
            StartCoroutine(LoadSceneWithDelay());
        }

        protected virtual IEnumerator LoadSceneWithDelay()
        {
            if (LoadedEvent != null)
            {
                LoadedEvent.Invoke();
            }

            yield return new WaitForSeconds(delayBeforeLoad);

            //AssetWarehouse.LoadLevel(LevelCrateToLoad.Barcode);
        }

        public void OnValidate()
        {
            if (LevelCrateToLoad.crate) LevelCrateToLoad.Barcode = LevelCrateToLoad.crate.Barcode;
        }
    }
}