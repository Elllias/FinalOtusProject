using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Components;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Mechanics
{
    public class ChaseMechanic
    {
        public event Action ChasingCompleted;
        public event Action ChasingResumed;
        public event Action ChasingStarted;
        public event Action ChasingStopped;
        
        private readonly ChaseComponent _chaseComponent;
        private bool _wasNear;

        private CancellationTokenSource _source;

        public ChaseMechanic(ChaseComponent chaseComponent)
        {
            _chaseComponent = chaseComponent;
            
            _chaseComponent.GetAgent().updateRotation = true;
        }

        public async void StartAsync()
        {
            ChasingStarted?.Invoke();
            
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
                        ChasingCompleted?.Invoke();
                    }
                }
                else if (_wasNear && distance > detectionDistance)
                {
                    _wasNear = false;

                    ChasingResumed?.Invoke();
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
            ChasingStopped?.Invoke();
            
            var agent = _chaseComponent.GetAgent();
            
            if (agent.isOnNavMesh)
                agent.isStopped = true;
            
            _source?.Cancel();
        }

        public async void MoveToAsync(Vector3 position, Action onEnd)
        {
            ChasingStarted?.Invoke();
            
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
    }
}