using Core.Components;
using Core.Data;
using Core.Events;
using Modules.EventBusFeature;
using UnityEngine;

namespace Core.Mechanics
{
    public class EnemyAnimationMechanic
    {
        private readonly int _isChasing = Animator.StringToHash("IsChasing");
        private readonly int _speed = Animator.StringToHash("Speed");
        
        private readonly AnimationComponent _component;
        private readonly int _sourceId;

        public EnemyAnimationMechanic(AnimationComponent component, int sourceId)
        {
            _component = component;
            _sourceId = sourceId;
            
            EventBus.Subscribe<ChasingResumedEvent>(OnChasingResumed);
            EventBus.Subscribe<ChasingCompletedEvent>(OnChasingCompleted);
            EventBus.Subscribe<PlayerFoundedEvent>(OnPlayerFounded);
            EventBus.Subscribe<PlayerLostEvent>(OnPlayerLost);
        }

        ~EnemyAnimationMechanic()
        {
            EventBus.Unsubscribe<ChasingResumedEvent>(OnChasingResumed);
            EventBus.Unsubscribe<ChasingCompletedEvent>(OnChasingCompleted);
            EventBus.Unsubscribe<PlayerFoundedEvent>(OnPlayerFounded);
            EventBus.Unsubscribe<PlayerLostEvent>(OnPlayerLost);
        }
        
        private void OnChasingCompleted(ChasingCompletedEvent evt)
        {
            if (_sourceId != evt.SourceId)
                return;
            
            ChangeState(EAnimationState.Hit);
        }

        private void OnChasingResumed(ChasingResumedEvent evt)
        {
            if (_sourceId != evt.SourceId)
                return;
            
            ChangeState(EAnimationState.Chase);
        }
        
        private void OnPlayerLost(PlayerLostEvent evt)
        {
            if (_sourceId != evt.SourceId)
                return;
            
            ChangeState(EAnimationState.Idle);
        }

        private void OnPlayerFounded(PlayerFoundedEvent evt)
        {
            if (_sourceId != evt.SourceId)
                return;
            
            ChangeState(EAnimationState.Chase);
        }

        private void ChangeState(EAnimationState state)
        {
            switch (state)
            {
                case EAnimationState.Chase:
                    _component.GetAnimator().SetBool(_isChasing, true);
                    _component.GetAnimator().SetFloat(_speed, 1);
                    break;
                case EAnimationState.Hit:
                    _component.GetAnimator().SetBool(_isChasing, false);
                    _component.GetAnimator().SetFloat(_speed, 0);
                    break;
                case EAnimationState.Idle:
                    _component.GetAnimator().SetBool(_isChasing, true);
                    _component.GetAnimator().SetFloat(_speed, 0);
                    break;
            }
        }
    }
}