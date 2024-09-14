using System.Collections.Generic;
using Core.Components;
using Core.Events;
using Core.Mechanics;
using Modules.EventBusFeature;
using UnityEngine;

namespace Core.Other
{
    public class AmmoRepository
    {
        private readonly Dictionary<int, int> _clipAmmoRepository = new();
        private readonly Dictionary<int, int> _totalAmmoRepository = new();

        private int _weaponId;

        public AmmoRepository(WeaponsComponent component)
        {
            foreach (var data in component.GetWeaponsData())
            {
                if (data.Config != null)
                {
                    if (!data.Config.IsUnlimited)
                        _totalAmmoRepository[data.Id] = data.TotalAmmoCount - data.ClipAmmoCount;
                    else
                        _totalAmmoRepository[data.Id] = int.MaxValue/2;
                    
                    _clipAmmoRepository[data.Id] = data.ClipAmmoCount;
                }
                else
                {
                    Debug.LogError($"WeaponConfig is null for weapon ID {data.Id}");
                }
            }

            EventBus.Subscribe<ReplenishmentAmmoEvent>(OnReplenishmentAmmoEvent);
            EventBus.Subscribe<WeaponSwappingEvent>(OnWeaponSwappingEvent);
            EventBus.Subscribe<ShootingEvent>(OnShootingEvent);
        }

        ~AmmoRepository()
        {
            EventBus.Unsubscribe<ReplenishmentAmmoEvent>(OnReplenishmentAmmoEvent);
            EventBus.Unsubscribe<WeaponSwappingEvent>(OnWeaponSwappingEvent);
            EventBus.Subscribe<ShootingEvent>(OnShootingEvent);
        }

        public int ReleaseAmmo(int value)
        {
            if (_totalAmmoRepository.TryGetValue(_weaponId, out var ammoCount))
            {
                if (ammoCount >= value)
                {
                    _totalAmmoRepository[_weaponId] -= value;
                    _clipAmmoRepository[_weaponId] += value;
                    return value;
                }
                else
                {
                    var ammo = _totalAmmoRepository[_weaponId];
                    _totalAmmoRepository[_weaponId] = 0;
                    _clipAmmoRepository[_weaponId] += ammo;
                    return ammo;
                }
            }

            Debug.LogWarning($"Ammo repository does not contain ammo for weapon ID: {_weaponId}");
            return 0;
        }
        
        public int GetClipAmmoCount(int weaponId)
        {
            return _clipAmmoRepository[weaponId];
        }

        public int GetTotalAmmoCount(int weaponId)
        {
            return _totalAmmoRepository[weaponId];
        }

        private void OnReplenishmentAmmoEvent(ReplenishmentAmmoEvent evt)
        {
            if (_totalAmmoRepository.ContainsKey(_weaponId))
            {
                _totalAmmoRepository[_weaponId] += evt.Ammo;
            }
            else
            {
                Debug.LogError($"{_weaponId} нет в репозитории");
            }
        }
        
        private void OnWeaponSwappingEvent(WeaponSwappingEvent evt)
        {
            _weaponId = evt.Data.Id;
        }
        
        private void OnShootingEvent(ShootingEvent _)
        {
            _clipAmmoRepository[_weaponId] -= 1;
        }
    }
}
