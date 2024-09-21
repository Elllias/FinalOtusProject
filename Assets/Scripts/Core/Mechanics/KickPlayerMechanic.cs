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
        private readonly int _sourceId;

        private CancellationTokenSource _source;

        public KickPlayerMechanic(KickPlayerComponent component, int sourceId)
        {
            _selfTransform = component.GetTransform();
            _cooldown = component.GetCooldown();
            _sourceId = sourceId;
            
            EventBus.Subscribe<ChasingResumedEvent>(OnChasingResumed);
            EventBus.Subscribe<ChasingCompletedEvent>(OnChasingCompleted);
        }

        ~KickPlayerMechanic()
        {
            EventBus.Unsubscribe<ChasingResumedEvent>(OnChasingResumed);
            EventBus.Unsubscribe<ChasingCompletedEvent>(OnChasingCompleted);
        }

        private async void StartAsync()
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
            _source = null;
        }

        public void Stop()
        {
            _source?.Cancel();
        }
        
        private void OnChasingCompleted(ChasingCompletedEvent evt)
        {
            if (_sourceId != evt.SourceId)
                return;
            
            StartAsync();
        }

        private void OnChasingResumed(ChasingResumedEvent evt)
        {
            if (_sourceId != evt.SourceId)
                return;
            
            Stop();
        }
    }
}