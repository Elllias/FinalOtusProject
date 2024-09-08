using System;
using Modules.HealthBarFeature;
using UnityEngine;

namespace Core
{
    public class ApplicationBootstrapper : MonoBehaviour
    {
        [SerializeField] private HealthBarView _healthBarView;
        private HealthBarObserver _healthBarObserver;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            
            _healthBarObserver = new HealthBarObserver(_healthBarView);
        }
    }
}