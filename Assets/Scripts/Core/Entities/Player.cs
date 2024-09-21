using Core.Components;
using Core.Events;
using Core.Mechanics;
using Core.Other;
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
        [SerializeField] private StepAudioComponent _stepAudioComponent;
        [SerializeField] private WeaponsComponent _weaponsComponent;

        [Header("Temp")] [SerializeField] private float _moveSpeed;
        [SerializeField] private float _mouseSensitivity;
        
        private ShootingMechanic _shootingMechanic;
        private ShootingVisualMechanic _shootingVisualMechanic;
        private ShootingAudioMechanic _shootingAudioMechanic;
        private ShootingAnimationMechanic _shootingAnimationMechanic;
        private MoveThroughInputMechanic _moveThroughInputMechanic;
        private RotateThroughInputMechanic _rotateThroughInputMechanic;
        private PlayerWalkAudioMechanic _walkAudioMechanic;
        private ReloadingMechanic _reloadingMechanic;
        private WeaponSwappingMechanic _weaponSwappingMechanic;
        private AmmoRepositoryProvider _ammoRepository;

        private void Awake()
        {
            _ammoRepository = new AmmoRepositoryProvider(_weaponsComponent);
            _shootingMechanic = new ShootingMechanic(_shootingComponent);
            _shootingVisualMechanic = new ShootingVisualMechanic(_shootingComponent);
            _shootingAudioMechanic = new ShootingAudioMechanic(_shootingComponent);
            _shootingAnimationMechanic = new ShootingAnimationMechanic(_shootingComponent, _shootingRotateComponent);
            _moveThroughInputMechanic = new MoveThroughInputMechanic(_moveComponent, _moveSpeed);
            _rotateThroughInputMechanic = new RotateThroughInputMechanic(_rotateComponent, _mouseSensitivity);
            _walkAudioMechanic = new PlayerWalkAudioMechanic(_stepAudioComponent);
            _reloadingMechanic = new ReloadingMechanic(_ammoRepository);
            _weaponSwappingMechanic = new WeaponSwappingMechanic(_shootingComponent, _ammoRepository, _weaponsComponent);
        }

        private void Update()
        {
            _rotateThroughInputMechanic.Update();
            _shootingMechanic.Update();
            _weaponSwappingMechanic.Update();
        }
        
        private void FixedUpdate()
        {
            _moveThroughInputMechanic.FixedUpdate();
        }

        public void ChangeHealth(int delta)
        {
            _healthComponent.ChangeHealth(delta);
            
            EventBus.RaiseEvent(new PlayerHealthChanged(_healthComponent.GetNormalizedHealth()));
        }
    }
}