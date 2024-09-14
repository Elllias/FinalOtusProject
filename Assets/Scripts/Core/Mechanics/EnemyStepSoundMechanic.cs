using System;
using Core.Components;
using Core.Events;
using Cysharp.Threading.Tasks;
using Modules.EventBusFeature;
using UnityEngine;

namespace Core.Mechanics
{
    public class EnemyStepSoundMechanic
    {
        private readonly ChaseMechanic _chaseMechanic;
        private readonly AudioSource _source;
        private readonly float _soundCooldown;

        private bool _isPlaying;

        public EnemyStepSoundMechanic(ChaseMechanic chaseMechanic, StepAudioComponent stepAudioComponent)
        {
            _chaseMechanic = chaseMechanic;
            _source = stepAudioComponent.Source;
            _soundCooldown = stepAudioComponent.SoundCooldown;

            _chaseMechanic.ChasingStarted += OnChasingStarted;
            _chaseMechanic.ChasingResumed += OnChasingStarted;
            _chaseMechanic.ChasingCompleted += OnChasingCompleted;
            _chaseMechanic.ChasingStopped += OnChasingCompleted;
        }

        ~EnemyStepSoundMechanic()
        {
            _chaseMechanic.ChasingStarted -= OnChasingStarted;
            _chaseMechanic.ChasingResumed -= OnChasingStarted;
            _chaseMechanic.ChasingCompleted -= OnChasingCompleted;
            _chaseMechanic.ChasingStopped -= OnChasingCompleted;
        }

        private void OnChasingCompleted()
        {
            _isPlaying = false;
        }

        private void OnChasingStarted()
        {
            StartEventSending();
        }

        private async void StartEventSending()
        {
            _isPlaying = true;

            while (_isPlaying)
            {
                _source.Play();

                await UniTask.WaitForSeconds(_soundCooldown);
            }
        }
    }
}