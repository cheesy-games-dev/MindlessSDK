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

            yield return SceneManager.Instance.LoadLevel(LevelCrateToLoad.Crate);
        }

        public void OnValidate()
        {
            if (LevelCrateToLoad.Crate) LevelCrateToLoad.Barcode = LevelCrateToLoad.Crate.Barcode;
        }

        public void Start()
        {
            OnValidate();
        }
    }
}