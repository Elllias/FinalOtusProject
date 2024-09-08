using System.Collections.Generic;
using Core.Components;
using Core.Events;
using Core.Mechanics;
using Modules.EventBusFeature;
using UnityEngine;

namespace Core
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private MoveComponent _moveComponent;
        [SerializeField] private RotateComponent _rotateComponent;
        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private ShootingComponent _shootingComponent;

        [Header("Temp")] [SerializeField] private float _moveSpeed;
        [SerializeField] private float _mouseSensitivity;

        private MoveThroughInputMechanic _moveThroughInputMechanic;
        private RotateThroughInputMechanic _rotateThroughInputMechanic;
        private ShootMechanic _shootMechanic;

        private void Awake()
        {
            _shootMechanic = new ShootMechanic(_shootingComponent);
            _moveThroughInputMechanic = new MoveThroughInputMechanic(_moveComponent, _moveSpeed);
            _rotateThroughInputMechanic = new RotateThroughInputMechanic(_rotateComponent, _mouseSensitivity);
        }

        private void Update()
        {
            _moveThroughInputMechanic.Update();
            _rotateThroughInputMechanic.Update();
        }

        public void ChangeHealth(int delta)
        {
            _healthComponent.ChangeHealth(delta);
            
            EventBus.RaiseEvent(new PlayerHealthChanged(_healthComponent.GetHealth()));
        }
    }
}