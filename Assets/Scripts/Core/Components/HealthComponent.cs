using System;
using UnityEngine;

namespace Core.Components
{
    [Serializable]
    public class HealthComponent
    {
        public event Action Death;
        
        [SerializeField] private int _currentHealth;
        [SerializeField] private int _maxHealth;

        public void ChangeHealth(int delta)
        {
            _currentHealth += delta;
            
            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                
                Debug.LogWarning("Somebody is dead!");
                Death?.Invoke();
            }
            else if (_currentHealth >= _maxHealth)
            {
                _currentHealth = _maxHealth;
            }
        }

        public void ChangeMaxHealth(int delta)
        {
            _maxHealth += delta;
        }

        public void ResetHealth()
        {
            _currentHealth = _maxHealth;
        }

        public float GetNormalizedHealth()
        {
            return Mathf.Clamp01(_currentHealth / (float)_maxHealth);
        }
    }
}