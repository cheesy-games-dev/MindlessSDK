using System;
using System.Collections.Generic;
using UnityEngine;

namespace MindlessSDK
{
    [DisallowMultipleComponent]
    public class Poolee : MindlessBehaviour, IValidate
    {
        public static List<Poolee> Poolees = new();
        public bool isSpawned;
        public override void Start()
        {
            isSpawned = false;
            Spawn();
        }

        public override void Validate()
        {
            Poolees.Clear();
            isSpawned = false;
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
