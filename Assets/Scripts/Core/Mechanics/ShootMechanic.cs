using System;
using Core.Components;
using Core.Entities;
using Core.Events;
using Modules.EventBusFeature;
using UnityEngine;

namespace Core.Mechanics
{
    public class ShootMechanic
    {
        private readonly Transform _shootingPoint;
        private readonly float _range;
        private readonly float _shootCooldown;

        private DateTime _lastShootTime;

        public ShootMechanic(ShootingComponent shootingComponent)
        {
            _shootingPoint = shootingComponent.GetPoint();
            _range = shootingComponent.GetRange();
            _shootCooldown = shootingComponent.GetShootCooldown();
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if ((DateTime.Now - _lastShootTime).TotalSeconds < _shootCooldown)
                {
                    return;
                }
            
                _lastShootTime = DateTime.Now;
                
                Shoot();
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
                
                // эффект попадания просто в обычный объект
                /*GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, 2f);*/
            }
            
            EventBus.RaiseEvent(new ShootEvent());
        }
    }
}