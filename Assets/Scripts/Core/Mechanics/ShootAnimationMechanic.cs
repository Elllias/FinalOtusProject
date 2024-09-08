using System.Threading.Tasks;
using Core.Components;
using Core.Events;
using Modules.EventBusFeature;
using UnityEngine;

namespace Core.Mechanics
{
    public class ShootAnimationMechanic
    {
        private readonly ShootingComponent _component;
        private readonly RotateComponent _rotateComponent;

        public ShootAnimationMechanic(ShootingComponent component, RotateComponent rotateComponent)
        {
            _component = component;
            _rotateComponent = rotateComponent;

            EventBus.Subscribe<ShootEvent>(OnShootEvent);
        }

        ~ShootAnimationMechanic()
        {
            EventBus.Unsubscribe<ShootEvent>(OnShootEvent);
        }

        private async void OnShootEvent(ShootEvent evt)
        {
            _component.GetVisualEffect().Play();
            
            var recoilForce = _component.GetRecoilForce();
            var duration = _component.GetShootDuration();
            var peakTime = duration * 0.3f;

            var originalRotation = _rotateComponent.GetCurrentRotation();
        
            var elapsed = 0f;
            while (elapsed < duration)
            {
                originalRotation = _rotateComponent.GetCurrentRotation();
                elapsed += Time.deltaTime;

                var peakT = Mathf.Clamp01((elapsed - peakTime) / (duration - peakTime));
                var easeInOut = Mathf.SmoothStep(0, 1, peakT);

                _rotateComponent.Rotate(elapsed <= peakTime
                    ? Quaternion.Slerp(Quaternion.Euler(originalRotation), Quaternion.Euler(originalRotation + new Vector3(-recoilForce, 0, 0)), easeInOut)
                    : Quaternion.Slerp(Quaternion.Euler(originalRotation + new Vector3(-recoilForce, 0, 0)), Quaternion.Euler(originalRotation), easeInOut));

                await Task.Yield(); 
            }
            
            _rotateComponent.Rotate(Quaternion.Euler(originalRotation));
        }
    }
}