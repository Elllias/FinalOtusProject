using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Components;
using Core.Entities;
using Core.Events;
using Modules.EventBusFeature;
using UnityEngine;

namespace Core.Mechanics
{
    public class KickPlayerMechanic
    {
        private readonly Transform _selfTransform;
        private readonly float _cooldown;
        
        private CancellationTokenSource _source;

        public KickPlayerMechanic(Transform selfTransform, float cooldown)
        {
            _selfTransform = selfTransform;
            _cooldown = cooldown;
        }

        public async void StartAsync()
        {
            _source = new CancellationTokenSource();
            
            while (!_source.IsCancellationRequested)
            {
                if (Physics.Raycast(_selfTransform.position, _selfTransform.forward, out var hit))
                {
                    if (hit.transform.TryGetComponent<Player>(out var player))
                    {
                        player.ChangeHealth(-1);
                    }
                }
                
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(_cooldown), _source.Token);
                }
                catch
                {
                    return;
                }
            }
            
            _source?.Dispose();
        }

        public void Stop()
        {
            _source?.Cancel();
        }
    }
}