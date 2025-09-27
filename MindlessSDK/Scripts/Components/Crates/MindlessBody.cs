using System.Collections.Generic;
using UnityEngine;

namespace MindlessSDK
{
    [DisallowMultipleComponent]
    public class MindlessBody : MindlessBehaviour, IValidate
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
