﻿using System;
using UnityEngine;
using UnityEngine.AI;

namespace Core.Components
{
    [Serializable]
    public class ChaseComponent
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private float _detectionDistance;
        [SerializeField] private Transform _target;
        
        public NavMeshAgent GetAgent()
        {
            return _navMeshAgent;
        }

        public Transform GetTarget()
        {
            return _target;
        }
        
        public void SetTarget(Transform target)
        {
            _target = target;
        }

        public float GetDetectionDistance()
        {
            return _detectionDistance;
        }
    }
}