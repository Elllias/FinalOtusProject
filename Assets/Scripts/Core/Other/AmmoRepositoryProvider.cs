using System.Collections.Generic;
using Core.Components;
using Core.Events;
using Modules.AmmoRepositoryFeature;
using Modules.EventBusFeature;
using UnityEngine;

namespace Core.Other
{
    public class AmmoRepositoryProvider
    {
        private readonly AmmoRepository _repository;
        private int _weaponId;

        public AmmoRepositoryProvider(WeaponsComponent component)
        {
            _repository = GetAmmoRepository(component);
            
            EventBus.Subscribe<ReplenishmentAmmoEvent>(OnReplenishmentAmmoEvent);
            EventBus.Subscribe<WeaponSwappingEvent>(OnWeaponSwappingEvent);
            EventBus.Subscribe<ShootingEvent>(OnShootingEvent);
        }

        ~AmmoRepositoryProvider()
        {
            EventBus.Unsubscribe<ReplenishmentAmmoEvent>(OnReplenishmentAmmoEvent);
            EventBus.Unsubscribe<WeaponSwappingEvent>(OnWeaponSwappingEvent);
            EventBus.Unsubscribe<ShootingEvent>(OnShootingEvent);
        }

        public int ReleaseAmmo(int ammoCount)
        {
            return _repository.ReleaseAmmo(_weaponId, ammoCount);
        }
        
        public int GetClipAmmoCount(int weaponId)
        {
            return _repository.GetClipAmmoCount(weaponId);
        }

        public int GetTotalAmmoCount(int weaponId)
        {
            return _repository.GetTotalAmmoCount(weaponId);
        }
        
        private AmmoRepository GetAmmoRepository(WeaponsComponent component)
        {
            var totalAmmoRepository = new Dictionary<int, int>();
            var clipAmmoRepository = new Dictionary<int, int>();
            
            foreach (var data in component.GetWeaponsData())
            {
                if (data.Config != null)
                {
                    if (!data.Config.IsUnlimited)
                        totalAmmoRepository[data.Id] = data.TotalAmmoCount - data.ClipAmmoCount;
                    else
                        totalAmmoRepository[data.Id] = int.MaxValue/2;
                    
                    clipAmmoRepository[data.Id] = data.ClipAmmoCount;
                }
                else
                {
                    Debug.LogError($"WeaponConfig is null for weapon ID {data.Id}");
                }
            }
            
            return new AmmoRepository(clipAmmoRepository, totalAmmoRepository);
        }
        
        private void OnReplenishmentAmmoEvent(ReplenishmentAmmoEvent evt)
        {
            _repository.AddTotalAmmo(_weaponId, evt.Ammo);
        }
        
        private void OnWeaponSwappingEvent(WeaponSwappingEvent evt)
        {
            _weaponId = evt.Data.Id;
        }
        
        private void OnShootingEvent(ShootingEvent _)
        {
            _repository.RemoveClipAmmo(_weaponId, 1);
        }
    }
}