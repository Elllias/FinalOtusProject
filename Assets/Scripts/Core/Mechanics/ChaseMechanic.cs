using Core.Components;
using Core.Enemies;
using Core.Events;
using Modules.EventBusFeature;
using UnityEngine;
using UnityEngine.AI;

namespace Core.Mechanics
{
    public class ChaseMechanic
    {
        private readonly NavMeshAgent _agent;
        private readonly Transform _target;
        
        public ChaseMechanic(ChaseComponent chaseComponent)
        {
            _agent = chaseComponent.GetAgent();
            _target = chaseComponent.GetTarget();
        }
        
        public void Update()
        {
            _agent.SetDestination(_target.position);
            
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                EventBus.RaiseEvent(new ChasingCompleteEvent());
                return;
            }
            
            if (Vector3.Distance(_target.position, _agent.transform.position) > _agent.stoppingDistance)
            {
                EventBus.RaiseEvent(new ChasingStartedEvent());
            }
        }
    }
}