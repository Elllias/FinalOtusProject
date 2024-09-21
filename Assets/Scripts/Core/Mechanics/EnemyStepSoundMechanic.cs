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
        private readonly int _sourceId;
        private readonly AudioSource _source;
        private readonly float _soundCooldown;

        private bool _isPlaying;

        public EnemyStepSoundMechanic(StepAudioComponent stepAudioComponent, int sourceId)
        {
            _sourceId = sourceId;
            _source = stepAudioComponent.Source;
            _soundCooldown = stepAudioComponent.SoundCooldown;

            EventBus.Subscribe<ChasingStartedEvent>(OnChasingStarted);
            EventBus.Subscribe<ChasingResumedEvent>(OnChasingResumed);
            EventBus.Subscribe<ChasingCompletedEvent>(OnChasingCompleted);
            EventBus.Subscribe<ChasingStoppedEvent>(OnChasingStopped);
        }

        ~EnemyStepSoundMechanic()
        {
            EventBus.Unsubscribe<ChasingStartedEvent>(OnChasingStarted);
            EventBus.Unsubscribe<ChasingResumedEvent>(OnChasingResumed);
            EventBus.Unsubscribe<ChasingCompletedEvent>(OnChasingCompleted);
            EventBus.Unsubscribe<ChasingStoppedEvent>(OnChasingStopped);
        }

        private void OnChasingCompleted(ChasingCompletedEvent evt)
        {
            if (evt.SourceId != _sourceId)
                return;
            
            _isPlaying = false;
        }

        private void OnChasingStarted(ChasingStartedEvent evt)
        {
            if (evt.SourceId != _sourceId)
                return;
            
            StartEventSending();
        }

        private void OnChasingResumed(ChasingResumedEvent evt)
        {
            if (evt.SourceId != _sourceId)
                return;
            
            StartEventSending();
        }

        private void OnChasingStopped(ChasingStoppedEvent evt)
        {
            if (evt.SourceId != _sourceId)
                return;
            
            _isPlaying = false;
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