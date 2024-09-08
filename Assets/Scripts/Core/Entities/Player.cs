using Core.Components;
using Core.Events;
using Core.Mechanics;
using Modules.EventBusFeature;
using UnityEngine;

namespace Core.Entities
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private MoveComponent _moveComponent;
        [SerializeField] private RotateComponent _rotateComponent;
        [SerializeField] private RotateComponent _shootingRotateComponent;
        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private ShootingComponent _shootingComponent;

        [Header("Temp")] [SerializeField] private float _moveSpeed;
        [SerializeField] private float _mouseSensitivity;

        private MoveThroughInputMechanic _moveThroughInputMechanic;
        private RotateThroughInputMechanic _rotateThroughInputMechanic;
        private ShootMechanic _shootMechanic;
        private ShootAnimationMechanic _shootAnimationMechanic;

        private void Awake()
        {
            _shootMechanic = new ShootMechanic(_shootingComponent);
            _shootAnimationMechanic = new ShootAnimationMechanic(_shootingComponent, _shootingRotateComponent);
            _moveThroughInputMechanic = new MoveThroughInputMechanic(_moveComponent, _moveSpeed);
            _rotateThroughInputMechanic = new RotateThroughInputMechanic(_rotateComponent, _mouseSensitivity);
        }

        private void Update()
        {
            _moveThroughInputMechanic.Update();
            _rotateThroughInputMechanic.Update();
            _shootMechanic.Update();
        }

        public void ChangeHealth(int delta)
        {
            _healthComponent.ChangeHealth(delta);
            
            EventBus.RaiseEvent(new PlayerHealthChanged(_healthComponent.GetNormalizedHealth()));
        }
    }
}