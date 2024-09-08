using System;
using UnityEngine;

namespace Core.Components
{
    [Serializable]
    public class VisibilityFieldComponent
    {
        [SerializeField] private float _radius;
        [SerializeField] private Transform _target;
        [SerializeField] private Transform _visionPoint;
        [SerializeField] private float _memoryTime;

        public float GetRadius()
        {
            return _radius;
        }
        
        public Transform GetTarget()
        {
            return _target;
        }
        
        public Transform GetVisionPoint()
        {
            return _visionPoint;
        }
        
        public float GetMemoryTime()
        {
            return _memoryTime;
        }
    }
}