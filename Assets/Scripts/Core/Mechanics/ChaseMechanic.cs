using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Components;
using Core.Events;
using Cysharp.Threading.Tasks;
using Modules.EventBusFeature;
using UnityEngine;

namespace Core.Mechanics
{
    public class ChaseMechanic
    {
        private readonly ChaseComponent _chaseComponent;
        private readonly int _sourceId;
        private bool _wasNear;

        private CancellationTokenSource _source;

        public ChaseMechanic(ChaseComponent chaseComponent, int sourceId)
        {
            _chaseComponent = chaseComponent;
            _sourceId = sourceId;

            _chaseComponent.GetAgent().updateRotation = true;
            
            EventBus.Subscribe<PlayerFoundedEvent>(OnPlayerFoundedEvent);
            EventBus.Subscribe<PlayerLostEvent>(OnPlayerLostEvent);
        }

        public async void StartAsync()
        {
            EventBus.RaiseEvent(new ChasingStartedEvent { SourceId = _sourceId });

            _source = new CancellationTokenSource();

            var agent = _chaseComponent.GetAgent();
            var target = _chaseComponent.GetTarget();
            var detectionDistance = _chaseComponent.GetDetectionDistance();

            while (!_source.IsCancellationRequested)
            {
                agent.isStopped = false;
                agent.SetDestination(target.position);

                var distance = Vector3.Distance(target.position, agent.transform.position);

                if (distance <= detectionDistance)
                {
                    var lookPosition = target.position;
                    lookPosition.y = agent.transform.position.y;

                    agent.transform.LookAt(lookPosition);

                    if (!_wasNear)
                    {
                        _wasNear = true;
                        EventBus.RaiseEvent(new ChasingCompletedEvent { SourceId = _sourceId });
                    }
                }
                else if (_wasNear && distance > detectionDistance)
                {
                    _wasNear = false;

                    EventBus.RaiseEvent(new ChasingResumedEvent { SourceId = _sourceId });
                }

                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(0.3), _source.Token);
                }
                catch
                {
                    return;
                }
            }

            _source.Dispose();
            _source = null;
        }

        public void Stop()
        {
            EventBus.RaiseEvent(new ChasingStoppedEvent { SourceId = _sourceId });

            var agent = _chaseComponent.GetAgent();

            if (agent.isOnNavMesh)
                agent.isStopped = true;

            _source?.Cancel();
        }

        public async void MoveToAsync(Vector3 position, Action onEnd)
        {
            EventBus.RaiseEvent(new ChasingStartedEvent { SourceId = _sourceId });

            _source = new CancellationTokenSource();

            var agent = _chaseComponent.GetAgent();
            var detectionDistance = _chaseComponent.GetDetectionDistance();

            agent.SetDestination(position);

            while (!_source.IsCancellationRequested)
            {
                var distance = Vector3.Distance(position, agent.transform.position);

                if (distance <= detectionDistance)
                {
                    onEnd?.Invoke();
                    _source.Cancel();
                }

                await UniTask.Yield(PlayerLoopTiming.Update);
            }
        }
        
        private void OnPlayerLostEvent(PlayerLostEvent evt)
        {
            if (evt.SourceId != _sourceId)
                return;
            
            Stop();
        }

        private void OnPlayerFoundedEvent(PlayerFoundedEvent evt)
        {
            if (evt.SourceId != _sourceId)
                return;
            
            Stop();
            StartAsync();
        }
    }
}