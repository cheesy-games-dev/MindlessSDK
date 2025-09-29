using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MindlessSDK
{
    public class AssetSpawner : MonoBehaviour
    {
        public static AssetSpawner Instance;
        public static Action<AssetSpawner, GameObject> OnGlobalSpawnedCallback;

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

        public virtual async UniTask<GameObject> Spawn(GameObject prefab, Vector3 position = new(), Quaternion rotation = new(), Transform parent = null, Action<GameObject> callback = null)
        {
            var handle = Addressables.InstantiateAsync(prefab, position, rotation, parent, true);
            var task = handle.ToUniTask();
            await task;
            var result = handle.Result;
            callback?.Invoke(result);
            OnGlobalSpawnedCallback?.Invoke(this, result);
            return result;
        }

        public virtual GameObject Spawn(Crate crate, Vector3 position = new(), Quaternion rotation = new(), Transform parent = null, Action<GameObject> callback = null)
        {
            if (crate.CrateReference is not GameObject)
            {
                Debug.LogWarning("Crate reference is not a GameObject.");
                return null;
            }
            var crateReference = crate.CrateReference as GameObject;
            var task = Spawn(crateReference, position, rotation, parent, callback).AsTask();
            return task.Result;
        }
    }
}