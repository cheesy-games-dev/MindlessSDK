using System;
using System.Collections.Generic;
using UnityEngine;

namespace AnalogSDK
{
    public class AnalogPoolee : AnalogBehaviour
    {
        public static List<AnalogPoolee> Poolees;
        public bool isSpawned;
        void Awake()
        {
            isSpawned = false;
            Spawn();
        }

        public virtual void Spawn()
        {
            if (isSpawned) return;
            Poolees.Add(this);
            isSpawned = true;
        }

        void OnDestroy()
        {
            if (!isSpawned) return;
            Despawn();
        }

        public virtual void Despawn()
        {
            if (!isSpawned) return;
            Poolees.Remove(this);
            isSpawned = false;
        }
    }
}
