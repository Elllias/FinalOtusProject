using System;
using System.Threading;
using Core.Components;
using Core.Events;
using Core.Other;
using Cysharp.Threading.Tasks;
using Modules.EventBusFeature;
using UnityEngine;

namespace Core.Mechanics
{
    public class ReloadingMechanic
    {
        private readonly AmmoRepositoryProvider _repository;

        private float _reloadingTime;
        private CancellationTokenSource _source;

        public ReloadingMechanic(AmmoRepositoryProvider repository)
        {
            _repository = repository;
            
            EventBus.Subscribe<ReloadingRequest>(OnReloadingRequest);
            EventBus.Subscribe<WeaponSwappingEvent>(OnWeaponSwappingEvent);
        }

        ~ReloadingMechanic()
        {
            EventBus.Unsubscribe<ReloadingRequest>(OnReloadingRequest);
            EventBus.Unsubscribe<WeaponSwappingEvent>(OnWeaponSwappingEvent);
            
            _source?.Cancel();
            _source?.Dispose();
            _source = null;
        }

        private void OnWeaponSwappingEvent(WeaponSwappingEvent evt)
        {
            _reloadingTime = evt.Data.Config.ReloadingTime;
            
            _source?.Cancel();
        }

        private async void OnReloadingRequest(ReloadingRequest evt)
        {
            if (_source != null)
                return;
            
            _source = new CancellationTokenSource();
            
            try
            {
                await UniTask.WaitForSeconds(_reloadingTime, cancellationToken: _source.Token);
            }
            catch (OperationCanceledException)
            {
                EventBus.RaiseEvent(new ReloadingCanceledEvent());
                
                _source.Dispose();
                _source = null;
                return;
            }
            
            EventBus.RaiseEvent(new ReloadingEvent{Ammo = _repository.ReleaseAmmo(evt.Ammo)});
            
            _source.Dispose();
            _source = null;
        }
    }
}