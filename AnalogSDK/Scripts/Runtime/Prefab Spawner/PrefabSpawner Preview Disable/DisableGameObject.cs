using UnityEngine;
namespace AnalogSDK
{
    public class DisableGameObject : MonoBehaviour
    {
        void Start()
        {
            Destroy(gameObject);
        }
    }
}