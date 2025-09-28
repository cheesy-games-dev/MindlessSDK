using UnityEngine;
using UnityEngine.SceneManagement;
using UltEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.ResourceProviders;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace MindlessSDK
{

    [AddComponentMenu("Zones/Zone Items/Scene Chunk")]
    public class SceneChunk : ZoneItem
    {
        public StreamChunk streamChunk;
        public UltEvent OnChunkLoaded;
        public UltEvent OnChunkUnloaded;

        private static Dictionary<string, SceneAsset> globallyLoadedScenes = new();
        private Dictionary<string, SceneInstance> localLoadedScenes = new();
        private Dictionary<Rigidbody, bool> originalKinematicStates = new Dictionary<Rigidbody, bool>();

        public override void Trigger(SceneZone.TriggerOption option)
        {
            if (option == SceneZone.TriggerOption.PRIMARY)
            {
                StartCoroutine(LoadScenes());
            }
            else if (option == SceneZone.TriggerOption.SECONDARY)
            {
                StartCoroutine(UnloadScenes());
            }
        }

        private IEnumerator LoadScenes()
        {
            foreach (var sceneAsset in streamChunk.scenes)
            {
                string sceneName = sceneAsset.name;
                if (!globallyLoadedScenes.ContainsKey(sceneName))
                {
                    globallyLoadedScenes.Add(sceneName, sceneAsset);
                    var asyncLoad = SceneManager.Instance.LoadScene(sceneAsset, LoadSceneMode.Additive, false);
                    yield return asyncLoad;
                    var scene = asyncLoad.AsTask().Result;
                    localLoadedScenes.Add(sceneName, scene);
                    // Set the kinematic states for Rigidbody objects in the loaded scene
                    SetRigidbodiesKinematic(scene.Scene, false);
                    OnChunkLoaded.Invoke(); // Invoke the event after loading
                    Debug.Log("Loaded Scene: " + sceneName);
                }
            }
        }

        private IEnumerator UnloadScenes()
        {
            foreach (string sceneName in localLoadedScenes.Keys)
            {
                if (globallyLoadedScenes.ContainsKey(sceneName))
                {
                    // Set all rigidbodies to kinematic before unloading
                    localLoadedScenes.TryGetValue(sceneName, out var scene);
                    SetRigidbodiesKinematic(scene.Scene, true);
                    var asyncUnload = SceneManager.Instance.UnloadScene(scene);

                    yield return asyncUnload;

                    localLoadedScenes.Remove(sceneName);
                    globallyLoadedScenes.Remove(sceneName);
                    OnChunkUnloaded.Invoke(); // Invoke the event after unloading
                    Debug.Log("Unloaded Scene: " + sceneName);
                }
            }
            localLoadedScenes.Clear();
        }

        private void SetRigidbodiesKinematic(Scene scene, bool kinematic)
        {
            if (scene.isLoaded)
            {
                GameObject[] rootObjects = scene.GetRootGameObjects();
                foreach (GameObject obj in rootObjects)
                {
                    Rigidbody[] rigidbodies = obj.GetComponentsInChildren<Rigidbody>(true);
                    foreach (Rigidbody rb in rigidbodies)
                    {
                        if (kinematic)
                        {
                            if (!originalKinematicStates.ContainsKey(rb))
                            {
                                originalKinematicStates[rb] = rb.isKinematic;
                            }
                            rb.isKinematic = true; // Set to kinematic when unloading
                        }
                        else
                        {
                            if (originalKinematicStates.TryGetValue(rb, out bool originalState))
                            {
                                rb.isKinematic = originalState; // Restore original state
                                originalKinematicStates.Remove(rb); // Clean up the dictionary entry
                            }
                        }
                    }
                }
            }
        }
    }
}