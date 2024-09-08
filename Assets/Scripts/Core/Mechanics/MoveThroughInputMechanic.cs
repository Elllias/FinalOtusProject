using Core.Components;
using UnityEngine;

namespace Core.Mechanics
{
    public class MoveThroughInputMechanic
    {
        private readonly MoveComponent _moveComponent;
        private float _moveSpeed;

        public MoveThroughInputMechanic(MoveComponent moveComponent, float moveSpeed)
        {
            _moveComponent = moveComponent;
            _moveSpeed = moveSpeed;
        }

        public void Update()
        {
            var horizontalInput = Input.GetAxisRaw("Horizontal");
            var verticalInput = Input.GetAxisRaw("Vertical");

            var direction =
                _moveComponent.GetForwardDirection() * verticalInput
                + _moveComponent.GetRightDirection() * horizontalInput;

            direction.y = 0;

            _moveComponent.MoveTo(direction * _moveSpeed);
        }
    }
}