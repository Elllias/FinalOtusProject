using Core.Components;
using Core.Data;
using UnityEngine;

namespace Core.Mechanics
{
    public class EnemyAnimation
    {
        private readonly AnimationComponent _component;

        public EnemyAnimation(AnimationComponent component)
        {
            _component = component;
        }

        public void ChangeState(EAnimationState state)
        {
            switch (state)
            {
                case EAnimationState.Chase:
                    _component.GetAnimator().SetBool("IsChasing", true);
                    _component.GetAnimator().SetFloat("Speed", 1);
                    break;
                case EAnimationState.Hit:
                    _component.GetAnimator().SetBool("IsChasing", false);
                    _component.GetAnimator().SetFloat("Speed", 0);
                    break;
                case EAnimationState.Idle:
                    _component.GetAnimator().SetBool("IsChasing", true);
                    _component.GetAnimator().SetFloat("Speed", 0);
                    break;
            }
        }
    }
}