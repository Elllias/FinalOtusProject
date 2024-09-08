using Core.Components;
using Core.Enemies;
using Core.Events;
using Modules.EventBusFeature;
using UnityEngine;

namespace Core.Mechanics
{
    public class KickPlayerMechanic
    {
        private readonly Transform _selfTransform;
        private readonly float _range;

        public KickPlayerMechanic(Transform selfTransform, float range)
        {
            _selfTransform = selfTransform;
            _range = range;
            
            EventBus.Subscribe<ChasingCompleteEvent>(OnShootEvent);
        }

        ~KickPlayerMechanic()
        {
            EventBus.Unsubscribe<ChasingCompleteEvent>(OnShootEvent);
        }

        private async void OnShootEvent(ChasingCompleteEvent _)
        {
            while ()
            if (Physics.Raycast(_selfTransform.position, _selfTransform.forward, out var hit, _range))
            {
                Debug.Log(hit.transform.name);

                if (hit.transform.TryGetComponent<Player>(out var player))
                {
                    player.ChangeHealth(-1);
                }

                // эффект попадания просто в обычный объект
                /*GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, 2f);*/
            }
        }
    }
}