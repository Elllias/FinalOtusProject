using Core.Components;
using Core.Data;
using Core.Mechanics;
using UnityEngine;

namespace Core.Entities
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private ChaseComponent _chaseComponent;
        [SerializeField] private AnimationComponent _animationComponent;
        [SerializeField] private VisibilityFieldComponent _visibilityFieldComponent;

        private ChaseMechanic _chaseMechanic;
        private KickPlayerMechanic _kickPlayerMechanic;
        private EnemyAnimation _enemyAnimation;
        private VisibilityFieldMechanic _visibilityFieldMechanic;

        private void Awake()
        {
            var selfTransform = transform;
            
            _chaseMechanic = new ChaseMechanic(_chaseComponent);
            _kickPlayerMechanic = new KickPlayerMechanic(selfTransform, 0.5f);
            _visibilityFieldMechanic = new VisibilityFieldMechanic(_visibilityFieldComponent);
            _enemyAnimation = new EnemyAnimation(_animationComponent);
        }
        
        private void OnEnable()
        {
            _visibilityFieldMechanic.StartAsync();

            if (_visibilityFieldMechanic.IsTargetVisible())
                OnPlayerFounded();
            
            _healthComponent.Death += OnDeath;
            _chaseMechanic.ChasingResumed += OnChasingResumed;
            _chaseMechanic.ChasingCompleted += OnChasingCompleted;
            _visibilityFieldMechanic.PlayerFounded += OnPlayerFounded;
            _visibilityFieldMechanic.PlayerLost += OnPlayerLost;
        }

        private void OnDisable()
        {
            _enemyAnimation.ChangeState(EAnimationState.Idle);
            
            _visibilityFieldMechanic.Stop();
            _kickPlayerMechanic.Stop();
            _chaseMechanic.Stop();
            
            _healthComponent.Death -= OnDeath;
            _chaseMechanic.ChasingResumed -= OnChasingResumed;
            _chaseMechanic.ChasingCompleted -= OnChasingCompleted;
            _visibilityFieldMechanic.PlayerFounded -= OnPlayerFounded;
            _visibilityFieldMechanic.PlayerLost -= OnPlayerLost;
        }

        public void ChangeHealth(int delta)
        {
            _healthComponent.ChangeHealth(delta);
        }

        private void OnDeath()
        {
            OnDisable();
            Destroy(gameObject);
        }
        
        private void OnChasingCompleted()
        {
            _kickPlayerMechanic.StartAsync();
            _enemyAnimation.ChangeState(EAnimationState.Hit);
        }

        private void OnChasingResumed()
        {
            _kickPlayerMechanic?.Stop();
            _enemyAnimation.ChangeState(EAnimationState.Chase);
        }
        
        private void OnPlayerLost()
        {
            _chaseMechanic.Stop();
            _enemyAnimation.ChangeState(EAnimationState.Idle);
        }

        private void OnPlayerFounded()
        {
            _chaseMechanic.StartAsync();
            _enemyAnimation.ChangeState(EAnimationState.Chase);
        }
    }
}