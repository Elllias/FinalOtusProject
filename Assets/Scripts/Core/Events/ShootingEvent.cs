using UnityEngine;

namespace Core.Events
{
    public struct ShootingEvent
    {
        private readonly Transform _targetTransform;
        private readonly Vector3 _hitPoint;
        private readonly Quaternion _hitRotation;

        public ShootingEvent(Transform targetTransform, Vector3 hitPoint, Quaternion hitRotation)
        {
            _targetTransform = targetTransform;
            _hitPoint = hitPoint;
            _hitRotation = hitRotation;
        }

        public Transform GetTargetTransform()
        {
            return _targetTransform;
        }
        
        public Vector3 GetHitPoint()
        {
            return _hitPoint;
        }
        
        public Quaternion GetHitRotation()
        {
            return _hitRotation;
        }
    }
}