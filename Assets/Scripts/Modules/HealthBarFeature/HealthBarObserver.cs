using Core;
using Core.Events;
using Modules.EventBusFeature;
using UnityEngine;

namespace Modules.HealthBarFeature
{
    public class HealthBarObserver
    {
        private readonly HealthBarView _view;
        
        public HealthBarObserver(HealthBarView view)
        {
            _view = view;

            EventBus.Subscribe<PlayerHealthChanged>(UpdateHealth);
        }
        
        ~HealthBarObserver()
        {
            EventBus.Unsubscribe<PlayerHealthChanged>(UpdateHealth);
        }
        
        private void UpdateHealth(PlayerHealthChanged evt)
        {
            _view.SetSliderValue(evt.Health);
        }
    }
}