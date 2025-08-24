using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AnalogSDK
{
    public class AnalogEntity : ValidateBehaviour
    {
        public List<AnalogBody> bodies = new();
        public List<AnalogBehaviour> behaviours = new();
        public List<Collider> colliders = new();
        private List<IValidate> validates = new();
        public AnalogPoolee poolee;

        public override void Validate()
        {
            validates = GetComponentsInChildren<IValidate>().ToList();
            validates.Remove(this);
            validates.ForEach(ValidateForEach);
            bodies = GetComponentsInChildren<AnalogBody>().ToList();
            colliders = GetComponentsInChildren<Collider>().ToList();
            behaviours = GetComponentsInChildren<AnalogBehaviour>().ToList();
            TryGetComponent(out poolee);
        }

        private void ValidateForEach(IValidate validate)
        {
            validate.Validate();
        }

        public void Despawn() => Destroy(gameObject);
    }

    public interface IValidate
    {
        public void Start();
        public void Validate();
    }
    public abstract class ValidateBehaviour : MonoBehaviour, IValidate
    {
        public virtual void Start()
        {
            Validate();
        }

        public virtual void Validate()
        {
        }
    }
}
