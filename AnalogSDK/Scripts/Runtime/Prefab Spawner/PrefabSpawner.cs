using UnityEngine;
using UnityEditor;

namespace AnalogSDK
{

    [ExecuteInEditMode]
    public class PrefabSpawner : MonoBehaviour
    {
        [Header("Reset Prefab Preview by selecting none then selecting your prefab")]
        public GameObject prefabToSpawn;
        private GameObject previewObject;
        private GameObject spawnedPrefab;
        private bool previewUpdated = false;

        [Header("Scaling Settings")]
        public bool applyScaleOnSpawn = false;
        public float scaleFactor = 1f;

        void Start()
        {
            // Only instantiate the prefab during runtime (not during edit mode)
            if (prefabToSpawn != null && Application.isPlaying)
            {
                spawnedPrefab = Instantiate(prefabToSpawn, transform.position, transform.rotation, transform);
                spawnedPrefab.name = prefabToSpawn.name + "SpawnedPrefab"; // Set the name of the spawned prefab

                // Apply scaling if needed
                if (applyScaleOnSpawn)
                {
                    spawnedPrefab.transform.localScale *= scaleFactor;
                }

                // Disable the preview object when entering play mode
                if (previewObject != null)
                {
                    previewObject.SetActive(false);
                }
            }
        }

        // Function that removes non-visual components like scripts, colliders, and rigidbodies from the preview
        private void RemoveNonVisualComponents(GameObject preview)
        {
            MonoBehaviour[] behaviours = preview.GetComponentsInChildren<MonoBehaviour>();
            foreach (var behaviour in behaviours)
            {
                if (behaviour.GetType() != typeof(PrefabSpawner))
                {
                    DestroyImmediate(behaviour);
                }
            }

            Collider[] colliders = preview.GetComponentsInChildren<Collider>();
            foreach (var collider in colliders)
            {
                DestroyImmediate(collider);
            }

            Rigidbody[] rigidbodies = preview.GetComponentsInChildren<Rigidbody>();
            foreach (var rb in rigidbodies)
            {
                DestroyImmediate(rb);
            }
        }

        // This function updates the prefab preview in the editor
        private void UpdatePrefabPreview()
        {
            if (prefabToSpawn != null && !Application.isPlaying)
            {
                // Ensure only one preview exists under PrefabSpawner
                if (previewObject == null)
                {
                    previewObject = Instantiate(prefabToSpawn, transform.position, transform.rotation, transform);
                    previewObject.name = "PreviewObject"; // Rename the preview object
                    RemoveNonVisualComponents(previewObject);

                    // Add the DisableGameObject script to the preview object
                    previewObject.AddComponent<DisableGameObject>();
                }

                // Ensure the preview object is the only one under PrefabSpawner
                if (transform.childCount > 1)
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        Transform child = transform.GetChild(i);
                        if (child != previewObject.transform)
                        {
                            DestroyImmediate(child.gameObject); // Destroy other children
                        }
                    }
                }

                previewObject.transform.position = transform.position;
                previewObject.transform.rotation = transform.rotation;

                previewUpdated = true;
            }
            else
            {
                // Remove preview object when exiting edit mode
                if (previewObject != null)
                {
                    DestroyImmediate(previewObject);
                    previewUpdated = false;
                }
            }
        }

        void Update()
        {
            if (!Application.isPlaying)
            {
                UpdatePrefabPreview();
            }
        }

        private void OnDestroy()
        {
            // Clean up the preview object when the PrefabSpawner is destroyed
            if (previewObject != null)
            {
                DestroyImmediate(previewObject);
            }

            // If we're not in play mode, make sure the spawned prefab is deleted
            if (spawnedPrefab != null && !Application.isPlaying)
            {
                DestroyImmediate(spawnedPrefab);
            }
        }
    }
}