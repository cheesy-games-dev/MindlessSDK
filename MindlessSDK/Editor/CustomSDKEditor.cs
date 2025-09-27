using UnityEditor;
using UnityEngine;

namespace MindlessSDK.Editor
{
    public class CustomSDKEditor : UnityEditor.Editor
    {

        [MenuItem("MindlessSDK/SDK/Zones/SceneZone")]
        public static void CreateZone(MenuCommand command)
        {
            GameObject sceneZone = new GameObject("Scene Zone");

            BoxCollider boxCollider = sceneZone.AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;

            sceneZone.AddComponent<SceneZone>();
            sceneZone.AddComponent<ZoneLinks>();

            Undo.RegisterCreatedObjectUndo(sceneZone, "Create Scene Zone");

            Selection.activeObject = sceneZone;
        }

        [MenuItem("MindlessSDK/SDK/Zones/ZoneChunk")]
        public static void CreateZoneChunk(MenuCommand command)
        {
            GameObject sceneZonechunk = new GameObject("Scene Chunk");

            BoxCollider boxCollider = sceneZonechunk.AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;

            sceneZonechunk.AddComponent<SceneZone>();
            sceneZonechunk.AddComponent<ZoneLinks>();
            sceneZonechunk.AddComponent<SceneChunk>();

            Undo.RegisterCreatedObjectUndo(sceneZonechunk, "Create Scene Chunk");

            Selection.activeObject = sceneZonechunk;
        }

        [MenuItem("MindlessSDK/SDK/Crates/CrateSpawner")]
        public static void CreateCrateSpawner(MenuCommand command)
        {
            GameObject go = new GameObject("Crate Spawner");

            go.AddComponent<CrateSpawner>();

            Undo.RegisterCreatedObjectUndo(go, "Created Crate Spawner");

            Debug.Log("Created custom Prefab Spawner.");
        }

        [MenuItem("MindlessSDK/SDK/Crates/LevelCrate")]
        public static void CreateLevelCrate(MenuCommand command)
        {
            GameObject go = new GameObject("Level Crate");

            go.AddComponent<LevelLoader>();

            Undo.RegisterCreatedObjectUndo(go, "Created Level Crate");

            Debug.Log("Created custom level crate.");
        }


    }
}