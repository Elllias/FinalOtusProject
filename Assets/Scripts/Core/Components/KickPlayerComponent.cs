using System;
using UnityEngine;

namespace Core.Components
{
    [Serializable]
    public class KickPlayerComponent
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private float _cooldown;

        public Transform GetTransform()
        {
            return _transform;
        }
        
        public float GetCooldown()
        {
            return _cooldown;
        }
    }
}