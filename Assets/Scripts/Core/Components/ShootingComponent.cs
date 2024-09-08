using System;
using UnityEngine;

namespace Core.Components
{
    [Serializable]
    public class ShootingComponent
    {
        [SerializeField] private Transform _shootingPoint;
        [SerializeField] private float _range;

        public Transform GetPoint()
        {
            return _shootingPoint;
        }
        
        public float GetRange()
        {
            return _range;
        }
    }
}