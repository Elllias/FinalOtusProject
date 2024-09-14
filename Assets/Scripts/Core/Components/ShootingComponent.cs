using System;
using System.Collections.Generic;
using Core.Configs;
using Core.Data;
using UnityEngine;

namespace Core.Components
{
    [Serializable]
    public class ShootingComponent
    {
        [SerializeField] private Transform _shootingPoint;
        [SerializeField] private HitEffectConfig _hitEffectConfig;
        [SerializeField] private ParticleSystem _visualEffect;
        [SerializeField] private Transform _effectsPool;
        [SerializeField] private AudioSource _audioSource;
        
        public Transform GetPoint()
        {
            return _shootingPoint;
        }
        
        public ParticleSystem GetVisualEffect()
        {
            return _visualEffect;
        }
        
        public HitEffectConfig GetHitSettings()
        {
            return _hitEffectConfig;
        }
        
        public Transform GetEffectsPool()
        {
            return _effectsPool;
        }

        public AudioSource GetAudioSource()
        {
            return _audioSource;
        }
    }
}