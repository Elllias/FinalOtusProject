using System;
using Core.Components;
using Core.Events;
using Core.Other;
using Modules.EventBusFeature;
using UnityEngine;

namespace Core.Mechanics
{
    public class WeaponSwappingMechanic
    {
        private readonly ShootingComponent _shootingComponent;
        private readonly AmmoRepository _repository;
        private readonly WeaponsComponent _weaponsComponent;

        public WeaponSwappingMechanic(ShootingComponent shootingComponent, AmmoRepository repository, WeaponsComponent weaponsComponent)
        {
            _shootingComponent = shootingComponent;
            _repository = repository;
            _weaponsComponent = weaponsComponent;
            
            ChangeWeapon(0);
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ChangeWeapon(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ChangeWeapon(1);
            }
        }

        private void ChangeWeapon(int weaponIndex)
        {
            var weaponsData = _weaponsComponent.GetWeaponsData();

            if (weaponIndex > weaponsData.Count )
                throw new ArgumentException();

            for (var i = 0; i < weaponsData.Count; i++)
            {
                weaponsData[i].WeaponObject.SetActive(i == weaponIndex);
            }

            var data = weaponsData[weaponIndex];
            data.ClipAmmoCount = _repository.GetClipAmmoCount(data.Id);
            data.TotalAmmoCount = _repository.GetTotalAmmoCount(data.Id);
            
            _shootingComponent.GetVisualEffect().transform.localPosition = data.Config.ShootEffectPoint;
            
            EventBus.RaiseEvent(new WeaponSwappingEvent {Data = data});
        }
    }
}