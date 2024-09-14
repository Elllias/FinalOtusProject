using System;
using System.Threading;
using Core.Components;
using Core.Entities;
using Core.Events;
using Cysharp.Threading.Tasks;
using Modules.EventBusFeature;
using UnityEngine;

namespace Core.Mechanics
{
    public class ShootingMechanic
    {
        private readonly Transform _shootingPoint;
        
        private float _range;
        private float _shootCooldown;
        private bool _isAutomatic;
        private int _maxAmmoCount;
        
        private int _ammoCount;
        private DateTime _lastShootTime;

        private bool _canShoot;

        public ShootingMechanic(ShootingComponent shootingComponent)
        {
            _shootingPoint = shootingComponent.GetPoint();

            _canShoot = true;
            
            EventBus.Subscribe<ReloadingEvent>(OnReloadingEvent);
            EventBus.Subscribe<WeaponSwappingEvent>(OnWeaponSwappingEvent);
            EventBus.Subscribe<ReloadingCanceledEvent>(OnReloadingCanceledEvent);
        }

        ~ShootingMechanic()
        {
            EventBus.Unsubscribe<ReloadingEvent>(OnReloadingEvent);
            EventBus.Unsubscribe<WeaponSwappingEvent>(OnWeaponSwappingEvent);
            EventBus.Unsubscribe<ReloadingCanceledEvent>(OnReloadingCanceledEvent);
        }
        
        public void Update()
        {
            if (!_canShoot)
                return;
            
            var isMousePressed = _isAutomatic 
                ? Input.GetKey(KeyCode.Mouse0)
                : Input.GetKeyDown(KeyCode.Mouse0);
            
            if (isMousePressed)
            {
                if ((DateTime.Now - _lastShootTime).TotalSeconds < _shootCooldown)
                    return;

                _lastShootTime = DateTime.Now;
                
                if (_ammoCount <= 0)
                {
                    EventBus.RaiseEvent(new DrySoundEvent());
                    return;
                }

                _ammoCount -= 1;
                
                Shoot();
            }
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (_ammoCount == _maxAmmoCount)
                {
                    return;
                }
                
                _canShoot = false;
                
                EventBus.RaiseEvent(new ReloadingRequest{Ammo = _maxAmmoCount - _ammoCount});
            }
        }

        private void Shoot()
        {
            if (Physics.Raycast(_shootingPoint.position, _shootingPoint.forward, out var hit, _range))
            {
                Debug.Log(hit.transform.name);

                if (hit.transform.TryGetComponent<Enemy>(out var enemy))
                {
                    enemy.ChangeHealth(-1);
                }
            }
            
            var hitRotation = Quaternion.LookRotation(_shootingPoint.position - hit.point);
            
            EventBus.RaiseEvent(new ShootingEvent(hit.transform, hit.point, hitRotation));
        }
        
        private void OnWeaponSwappingEvent(WeaponSwappingEvent evt)
        {
            var config = evt.Data.Config;
            
            _range = config.Range;
            _shootCooldown = config.ShootCooldown;
            _isAutomatic = config.IsAutomatic;
            _maxAmmoCount = config.MaxAmmoCount;
            _ammoCount = evt.Data.ClipAmmoCount;

            _canShoot = true;
        }

        private void OnReloadingEvent(ReloadingEvent evt)
        {
            _canShoot = true;
            
            _ammoCount += evt.Ammo;
        }
        
        private void OnReloadingCanceledEvent(ReloadingCanceledEvent obj)
        {
            _canShoot = true;
        }
    }
}