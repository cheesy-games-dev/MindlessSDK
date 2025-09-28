using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace MindlessSDK
{
    public class SceneManager : MonoBehaviour
    {
        public static SceneManager Instance;

        void Start()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        public virtual SceneInstance LoadLevel(LevelCrate levelCrate, LoadSceneMode mode = LoadSceneMode.Single, Action<SceneInstance, LoadSceneMode> callback = null)
        {
            var handle = LoadScene(levelCrate._crateReference, mode, true, callback);
            return handle.AsTask().Result;
        }

        public virtual async UniTask<SceneInstance> LoadScene(SceneAsset sceneAsset, LoadSceneMode mode = LoadSceneMode.Single, bool allowSceneActivation = true, Action<SceneInstance, LoadSceneMode> callback = null)
        {
            var handle = Addressables.LoadSceneAsync(sceneAsset, mode, allowSceneActivation);
            await handle.ToUniTask();
            var scene = handle.Result;
            callback?.Invoke(scene, mode);
            return scene;
        }

        public virtual async UniTask<SceneInstance> UnloadScene(SceneInstance sceneInstance, UnloadSceneOptions options = UnloadSceneOptions.None, bool allowSceneActivation = true, Action<SceneInstance, UnloadSceneOptions> callback = null)
        {
            var handle = Addressables.UnloadSceneAsync(sceneInstance, allowSceneActivation);
            await handle.ToUniTask();
            var scene = handle.Result;
            callback?.Invoke(scene, options);
            return scene;
        }
    }
}
