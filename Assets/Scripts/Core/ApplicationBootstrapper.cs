using System;
using Core.Data;
using Core.Entities;
using Core.Other;
using Core.UI;
using Modules.AmmoPanelFeature;
using Modules.HealthBarFeature;
using UnityEngine;

namespace Core
{
    public class ApplicationBootstrapper : MonoBehaviour
    {
        [SerializeField] private HealthBarView _healthBarView;
        [SerializeField] private AmmoView _ammoView;
        [SerializeField] private EnemyFactoryData _enemyFactoryData;
        
        private HealthBarObserver _healthBarObserver;
        private AmmoViewObserver _ammoViewObserver;
        private EnemyFactory _enemyFactory;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            
            _healthBarObserver = new HealthBarObserver(_healthBarView);
            _ammoViewObserver = new AmmoViewObserver(_ammoView, 5);
            _enemyFactory = new EnemyFactory(_enemyFactoryData);
        }
    }
}