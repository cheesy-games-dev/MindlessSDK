using System.Collections;
using UltEvents;
using UnityEngine;

namespace MindlessSDK
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

        public virtual IEnumerator LoadSceneWithDelay()
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