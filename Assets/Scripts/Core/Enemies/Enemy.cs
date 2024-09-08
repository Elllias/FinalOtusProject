using Core.Components;
using Core.Mechanics;
using UnityEngine;

namespace Core.Enemies
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private ChaseComponent _chaseComponent;

        private ChaseMechanic _chaseMechanic;

        private void Awake()
        {
            _chaseMechanic = new ChaseMechanic(_chaseComponent);
        }

        private void Update()
        {
            _chaseMechanic.Update();
        }

        private void OnEnable()
        {
            _healthComponent.Death += OnDeath;
        }
        
        private void OnDisable()
        {
            _healthComponent.Death -= OnDeath;
        }

        public void ChangeHealth(int delta)
        {
            _healthComponent.ChangeHealth(delta);
        }

        private void OnDeath()
        {
            Destroy(gameObject);
        }
    }
}