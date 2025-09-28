using UnityEngine;
using UnityEngine.SceneManagement;

namespace MindlessSDK
{
    [CreateAssetMenu(fileName = "StreamChunk", menuName = "ScriptableObjects/StreamChunk", order = 1)]
    public class StreamChunk : ScriptableObject
    {
        public SceneAsset[] scenes;

        // Method to get scene names
        public string[] GetSceneNames()
        {
            string[] sceneNames = new string[scenes.Length];
            for (int i = 0; i < scenes.Length; i++)
            {
                sceneNames[i] = scenes[i].name;
            }
            return sceneNames;
        }
    }
}