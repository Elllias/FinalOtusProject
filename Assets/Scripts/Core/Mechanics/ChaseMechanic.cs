using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Components;
using Core.Events;
using Modules.EventBusFeature;
using UnityEngine;
using UnityEngine.AI;

namespace Core.Mechanics
{
    public class ChaseMechanic
    {
        public event Action ChasingCompleted;
        public event Action ChasingResumed;
        
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
            _source = new CancellationTokenSource();
            
            var agent = _chaseComponent.GetAgent();
            var target = _chaseComponent.GetTarget();
            
            while (!_source.IsCancellationRequested)
            {
                agent.isStopped = false;
                agent.SetDestination(target.position);

                var distance = Vector3.Distance(target.position, agent.transform.position);

                if (distance <= agent.stoppingDistance)
                {
                    agent.transform.LookAt(target);

                    if (!_wasNear)
                    {
                        _wasNear = true;
                        ChasingCompleted?.Invoke();
                    }
                }
                else if (_wasNear && distance > agent.stoppingDistance)
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
            
            _source?.Dispose();
        }
        
        public void Stop()
        {
            var agent = _chaseComponent.GetAgent();
            
            if (agent.isOnNavMesh)
                agent.isStopped = true;
            
            _source?.Cancel();
        }
    }
}