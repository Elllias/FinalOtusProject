using Core.Components;
using Core.Data;
using Core.Events;
using Core.Mechanics;
using Modules.EventBusFeature;
using UnityEngine;

namespace Core.Entities
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private ChaseComponent _chaseComponent;
        [SerializeField] private AnimationComponent _animationComponent;
        [SerializeField] private VisibilityFieldComponent _visibilityFieldComponent;
        [SerializeField] private StepAudioComponent _stepAudioComponent;
        [SerializeField] private KickPlayerComponent _kickPlayerComponent; 

        private ChaseMechanic _chaseMechanic;
        private KickPlayerMechanic _kickPlayerMechanic;
        private EnemyAnimationMechanic _enemyAnimationMechanic;
        private VisibilityFieldMechanic _visibilityFieldMechanic;
        private EnemyStepSoundMechanic _enemyStepSoundMechanic;
        private int _sourceId;

        private void Awake()
        {
            _sourceId = GetHashCode();

            _enemyAnimationMechanic = new EnemyAnimationMechanic(_animationComponent, _sourceId);
            _chaseMechanic = new ChaseMechanic(_chaseComponent, _sourceId);
            _kickPlayerMechanic = new KickPlayerMechanic(_kickPlayerComponent, _sourceId);
            _visibilityFieldMechanic = new VisibilityFieldMechanic(_visibilityFieldComponent, _sourceId);
            _enemyStepSoundMechanic = new EnemyStepSoundMechanic(_stepAudioComponent, _sourceId);
        }
        
        private void OnEnable()
        {
            _visibilityFieldMechanic.StartAsync();

            if (_visibilityFieldMechanic.IsTargetVisible())
                EventBus.RaiseEvent(new PlayerFoundedEvent{SourceId = _sourceId});
            
            _healthComponent.Death += OnDeath;
        }

        private void OnDisable()
        {
            _visibilityFieldMechanic.Stop();
            _kickPlayerMechanic.Stop();
            _chaseMechanic.Stop();
            
            _healthComponent.Death -= OnDeath;
        }

        public void ChangeHealth(int delta)
        {
            _healthComponent.ChangeHealth(delta);
        }

        public void MoveTo(Vector3 position)
        {
            _chaseMechanic.MoveToAsync(position, 
                () => EventBus.RaiseEvent(new PlayerLostEvent{SourceId = _sourceId}));
        }
        
        public void Initialize()
        {
            _healthComponent.ResetHealth();
        }

        private void OnDeath()
        {
            EventBus.RaiseEvent(new ReplenishmentAmmoEvent {Ammo = 10});
            EventBus.RaiseEvent(new EnemyDeathEvent {Source = this});
        }
    }
}