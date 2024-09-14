using System.Linq;
using Core.Components;
using Core.Events;
using Core.Factories;
using Modules.EventBusFeature;
using UnityEditor;

namespace Core.Mechanics
{
    public class ShootingVisualMechanic
    {
        private readonly ShootingComponent _component;
        private readonly HitEffectFactory _effectFactory;

        public ShootingVisualMechanic(ShootingComponent component)
        {
            _component = component;
            _effectFactory = new HitEffectFactory(_component.GetHitSettings(), _component.GetEffectsPool());

            EventBus.Subscribe<ShootingEvent>(OnShootEvent);
        }

        ~ShootingVisualMechanic()
        {
            EventBus.Unsubscribe<ShootingEvent>(OnShootEvent);
        }

        private void OnShootEvent(ShootingEvent evt)
        {
            _component.GetVisualEffect().Play();

            var target = evt.GetTargetTransform();
            var hitPoint = evt.GetHitPoint();
            var hitRotation = evt.GetHitRotation();

            if (hitPoint == default || target == null || hitRotation == default) return;

            _effectFactory.SpawnEffect(target.gameObject.layer, hitPoint, hitRotation);
        }
    }
}