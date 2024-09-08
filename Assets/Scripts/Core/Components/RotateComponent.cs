using System;
using UnityEngine;

namespace Core.Components
{
    [Serializable]
    public class RotateComponent
    {
        [SerializeField] private Transform _transform;
        
        public void Rotate(Quaternion rotation)
        {
            _transform.localRotation = rotation;
        }

        public Vector3 GetCurrentRotation()
        {
            return _transform.localRotation.eulerAngles;
        }
    }
}