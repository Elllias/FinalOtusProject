using System.Threading.Tasks;
using Core.Components;
using Core.Events;
using Modules.EventBusFeature;
using UnityEngine;

namespace Core.Mechanics
{
    public class ShootingAnimationMechanic
    {
        private readonly RotateComponent _rotateComponent;
        private readonly ParticleSystem _particleSystem;
        private float _recoilForce;
        private float _shootDuration;

        public ShootingAnimationMechanic(ShootingComponent component, RotateComponent rotateComponent)
        {
            _rotateComponent = rotateComponent;
            _particleSystem = component.GetVisualEffect();

            EventBus.Subscribe<ShootingEvent>(OnShootEvent);
            EventBus.Subscribe<WeaponSwappingEvent>(OnWeaponSwappingEvent);
        }

        ~ShootingAnimationMechanic()
        {
            EventBus.Unsubscribe<ShootingEvent>(OnShootEvent);
            EventBus.Unsubscribe<WeaponSwappingEvent>(OnWeaponSwappingEvent);
        }

        private async void OnShootEvent(ShootingEvent evt)
        {
            _particleSystem.Play();
            
            var peakTime = _shootDuration * 0.3f;

            var originalRotation = _rotateComponent.GetCurrentRotation();
        
            var elapsed = 0f;
            while (elapsed < _shootDuration)
            {
                originalRotation = _rotateComponent.GetCurrentRotation();
                elapsed += Time.deltaTime;

                var peakT = Mathf.Clamp01((elapsed - peakTime) / (_shootDuration - peakTime));
                var easeInOut = Mathf.SmoothStep(0, 1, peakT);

                _rotateComponent.Rotate(elapsed <= peakTime
                    ? Quaternion.Slerp(Quaternion.Euler(originalRotation), Quaternion.Euler(originalRotation + new Vector3(-_recoilForce, 0, 0)), easeInOut)
                    : Quaternion.Slerp(Quaternion.Euler(originalRotation + new Vector3(-_recoilForce, 0, 0)), Quaternion.Euler(originalRotation), easeInOut));

                await Task.Yield(); 
            }
            
            _rotateComponent.Rotate(Quaternion.Euler(originalRotation));
        }
        
        private void OnWeaponSwappingEvent(WeaponSwappingEvent evt)
        {
            var config = evt.Data.Config;

            _recoilForce = config.RecoilForce;
            _shootDuration = config.ShootDuration;
        }
    }
}