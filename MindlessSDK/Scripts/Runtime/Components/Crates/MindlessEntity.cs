using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MindlessSDK
{
    [DisallowMultipleComponent]
    public class MindlessEntity : ValidateBehaviour
    {
        public List<MindlessBody> bodies = new();
        public List<MindlessBehaviour> behaviours = new();
        public List<Collider> colliders = new();
        private List<IValidate> validates = new();
        public Poolee poolee;

        public override void Validate()
        {
            AddValidates();
            AddBodies();
            colliders = GetComponentsInChildren<Collider>().ToList();
            behaviours = GetComponentsInChildren<MindlessBehaviour>().ToList();
            TryGetComponent(out poolee);
        }
        #region  add
        private void AddBodies()
        {
            GetComponentsInChildren<Rigidbody>().ToList().ForEach(AddAnalogBody);
            GetComponentsInChildren<ArticulationBody>().ToList().ForEach(AddAnalogBody);
            bodies = GetComponentsInChildren<MindlessBody>().ToList();
        }

        private void AddValidates()
        {
            validates = GetComponentsInChildren<IValidate>().ToList();
            validates.Remove(this);
            validates.ForEach(ValidateForEach);
        }

        private void AddAnalogBody(ArticulationBody body) => AddAnalogBody(body.gameObject);
        private void AddAnalogBody(Rigidbody body) => AddAnalogBody(body.gameObject);

        private void AddAnalogBody(GameObject body)
        {
            if (body.GetComponent<MindlessBody>()) return;
            body.AddComponent<MindlessBody>();
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
