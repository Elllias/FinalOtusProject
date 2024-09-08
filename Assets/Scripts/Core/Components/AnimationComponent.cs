using System;
using UnityEngine;

namespace Core.Components
{
    [Serializable]
    public class AnimationComponent
    {
        [SerializeField] private Animator _animator;

        public Animator GetAnimator()
        {
            return _animator;
        }
    }
}