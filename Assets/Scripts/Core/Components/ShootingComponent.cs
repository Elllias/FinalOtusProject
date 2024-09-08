using System;
using UnityEngine;

namespace Core.Components
{
    [Serializable]
    public class ShootingComponent
    {
        [SerializeField] private Transform _shootingPoint;
        [SerializeField] private float _recoilForce;
        [SerializeField] private float _shootDuration;
        [SerializeField] private float _shootCooldown;
        [SerializeField] private ParticleSystem _visualEffect;
        [SerializeField] private float _range;

        public Transform GetPoint()
        {
            return _shootingPoint;
        }
        
        public float GetRange()
        {
            return _range;
        }
        
        public float GetRecoilForce()
        {
            return _recoilForce;
        }

        public float GetShootDuration()
        {
            return _shootDuration;
        }
        
        public ParticleSystem GetVisualEffect()
        {
            return _visualEffect;
        }

        public float GetShootCooldown()
        {
            return _shootCooldown;
        }
    }
}