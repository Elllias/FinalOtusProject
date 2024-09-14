using Core.Components;
using Core.Events;
using Modules.EventBusFeature;
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

        public void FixedUpdate()
        {
            var horizontalInput = Input.GetAxisRaw("Horizontal");
            var verticalInput = Input.GetAxisRaw("Vertical");

            var direction =
                _moveComponent.GetForwardDirection() * verticalInput
                + _moveComponent.GetRightDirection() * horizontalInput;

            direction.y = 0;

            if (direction == Vector3.zero) return;

            _moveComponent.MoveTo(direction.normalized * _moveSpeed);
            
            EventBus.RaiseEvent(new PlayerStepEvent());
        }
    }
}