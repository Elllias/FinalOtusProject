using System;
using UnityEngine;

namespace Core.Components
{
    [Serializable]
    public class MoveComponent
    {
        [SerializeField] private Rigidbody _body;

        public void MoveTo(Vector3 direction)
        {
            _body.AddForce(direction);
        }

        public Vector3 GetForwardDirection()
        {
            return _body.transform.forward;
        }

        public Vector3 GetRightDirection()
        {
            return _body.transform.right;
        }
    }
}