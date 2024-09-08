using System;
using UnityEngine;

namespace Core.Components
{
    [Serializable]
    public class HealthComponent
    {
        public event Action Death;
        
        [SerializeField] private int _health;

        public void ChangeHealth(int delta)
        {
            _health += delta;

            if (_health <= 0)
            {
                Debug.LogWarning("Somebody is dead!");
                Death?.Invoke();
            }
        }

        public int GetHealth()
        {
            return _health;
        }
    }
}