using System;
using Core.Components;
using Core.Events;
using Modules.EventBusFeature;
using UnityEngine;

namespace Core.Mechanics
{
    public class PlayerWalkAudioMechanic
    {
        private readonly AudioSource _source;
        private readonly float _soundCooldown;

        private DateTime _lastSoundTime;
        
        public PlayerWalkAudioMechanic(StepAudioComponent component)
        {
            _source = component.Source;
            _soundCooldown = component.SoundCooldown;
            
            EventBus.Subscribe<PlayerStepEvent>(OnPlayerStepEvent);
        }

        ~PlayerWalkAudioMechanic()
        {
            EventBus.Unsubscribe<PlayerStepEvent>(OnPlayerStepEvent);
        }

        private void OnPlayerStepEvent(PlayerStepEvent _)
        {
            if ((DateTime.Now - _lastSoundTime).TotalSeconds < _soundCooldown)
                return;
                
            _lastSoundTime = DateTime.Now;
            
            _source.Play();
        }
    }
}