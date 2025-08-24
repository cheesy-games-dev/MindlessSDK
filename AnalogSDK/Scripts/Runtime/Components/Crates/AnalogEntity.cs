using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AnalogSDK
{
    public class AnalogEntity : MonoBehaviour
    {
        public List<AnalogBody> bodies;
        public List<AnalogBehaviour> behaviours;
        public List<Collider> colliders;
        public AnalogPoolee poolee;
        void OnValidate() => Validate();
        void Start()
        {
            Validate();
        }

        public void Validate()
        {
            bodies = GetComponentsInChildren<AnalogBody>().ToList();
            behaviours = GetComponentsInChildren<AnalogBehaviour>().ToList();
            TryGetComponent(out poolee);
        }

        public void Despawn() => Destroy(gameObject);
    }
}
