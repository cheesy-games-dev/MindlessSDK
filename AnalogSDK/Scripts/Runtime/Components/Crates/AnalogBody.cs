using System.Collections.Generic;
using UnityEngine;

namespace AnalogSDK
{
    public class AnalogBody : AnalogBehaviour, IValidate
    {
        public Object body => Rigidbody ? Rigidbody : ArticulationBody;
        [HideInInspector] public Rigidbody Rigidbody;
        [HideInInspector] public ArticulationBody ArticulationBody;
        public override void Validate()
        {
            if (!TryGetComponent(out Rigidbody)) TryGetComponent(out ArticulationBody);
        }
    }
}
