using System.Collections.Generic;
using UnityEngine;

namespace AnalogSDK
{
    public class AnalogBody : AnalogBehaviour
    {
        public Object body => Rigidbody ? Rigidbody : ArticulationBody;
        [HideInInspector] public Rigidbody Rigidbody;
        [HideInInspector] public ArticulationBody ArticulationBody;
    }
}
