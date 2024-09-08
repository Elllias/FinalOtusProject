using Core.Components;
using Core.Enemies;
using Core.Events;
using Modules.EventBusFeature;
using UnityEngine;

namespace Core.Mechanics
{
    public class ShootMechanic
    {
        private readonly Transform _shootingPoint;
        private readonly float _range;

        public ShootMechanic(ShootingComponent shootingComponent)
        {
            _shootingPoint = shootingComponent.GetPoint();
            _range = shootingComponent.GetRange();
            
            EventBus.Subscribe<ShootEvent>(OnShootEvent);
        }

        ~ShootMechanic()
        {
            EventBus.Unsubscribe<ShootEvent>(OnShootEvent);
        }

        private void OnShootEvent(ShootEvent _)
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
        }
    }
}