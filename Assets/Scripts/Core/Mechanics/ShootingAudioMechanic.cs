using Core.Components;
using Core.Events;
using Modules.EventBusFeature;
using UnityEngine;

namespace Core.Mechanics
{
    public class ShootingAudioMechanic
    {
        private readonly AudioSource _source;
        
        private AudioClip _shootSound;
        private AudioClip _drySound;
        private AudioClip _reloadSound;

        public ShootingAudioMechanic(ShootingComponent component)
        {
            _source = component.GetAudioSource();
            
            EventBus.Subscribe<ShootingEvent>(OnShootingEvent);
            EventBus.Subscribe<DrySoundEvent>(OnDrySoundEvent);
            EventBus.Subscribe<ReloadingRequest>(OnReloadingRequest);
            EventBus.Subscribe<WeaponSwappingEvent>(OnWeaponSwappingEvent);
            EventBus.Subscribe<ReloadingCanceledEvent>(OnReloadingCanceledEvent);
        }

        ~ShootingAudioMechanic()
        {
            EventBus.Unsubscribe<ShootingEvent>(OnShootingEvent);
            EventBus.Unsubscribe<DrySoundEvent>(OnDrySoundEvent);
            EventBus.Unsubscribe<ReloadingRequest>(OnReloadingRequest);
            EventBus.Unsubscribe<WeaponSwappingEvent>(OnWeaponSwappingEvent);
            EventBus.Unsubscribe<ReloadingCanceledEvent>(OnReloadingCanceledEvent);
        }

        private void OnWeaponSwappingEvent(WeaponSwappingEvent evt)
        {
            if (_source.isPlaying)
                _source.Stop();
            
            var config = evt.Data.Config;
            
            _shootSound = config.ShootSound;
            _drySound = config.DrySound;
            _reloadSound = config.ReloadSound;
        }

        private void OnShootingEvent(ShootingEvent _)
        {
            _source.clip = _shootSound;
            
            _source.Play();
        }
        
        private void OnDrySoundEvent(DrySoundEvent _)
        {
            _source.clip = _drySound;
            
            _source.Play();
        }
        
        private void OnReloadingRequest(ReloadingRequest evt)
        {
            if (evt.Ammo <= 0) return;
            
            _source.clip = _reloadSound;
            
            _source.Play();
        }
        
        private void OnReloadingCanceledEvent(ReloadingCanceledEvent obj)
        {
            _source.Stop();
        }
    }
}