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
            AddValidates();
            AddBodies();
            colliders = GetComponentsInChildren<Collider>().ToList();
            behaviours = GetComponentsInChildren<AnalogBehaviour>().ToList();
            TryGetComponent(out poolee);
        }
        #region  add
        private void AddValidates()
        {
            GetComponentsInChildren<Rigidbody>().ToList().ForEach(AddAnalogBody);
            GetComponentsInChildren<ArticulationBody>().ToList().ForEach(AddAnalogBody);
            bodies = GetComponentsInChildren<AnalogBody>().ToList();
        }

        private void AddBodies()
        {
            validates = GetComponentsInChildren<IValidate>().ToList();
            validates.Remove(this);
            validates.ForEach(ValidateForEach);
        }

        private void AddAnalogBody(ArticulationBody body) => AddAnalogBody(body.gameObject);
        private void AddAnalogBody(Rigidbody body) => AddAnalogBody(body.gameObject);

        private void AddAnalogBody(GameObject body)
        {
            if (TryGetComponent<AnalogBody>(out var analogbody)) Destroy(analogbody);
            body.AddComponent<AnalogBody>();
        }
        #endregion
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
