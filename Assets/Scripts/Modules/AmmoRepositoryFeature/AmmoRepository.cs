using System.Collections.Generic;
using UnityEngine;

namespace Modules.AmmoRepositoryFeature
{
    public class AmmoRepository
    {
        private readonly Dictionary<int, int> _clipAmmoRepository;
        private readonly Dictionary<int, int> _totalAmmoRepository;

        public AmmoRepository(Dictionary<int, int> clipAmmoRepository, Dictionary<int, int> totalAmmoRepository)
        {
            _clipAmmoRepository = clipAmmoRepository;
            _totalAmmoRepository = totalAmmoRepository;
        }

        public int ReleaseAmmo(int weaponId, int value)
        {
            if (_totalAmmoRepository.TryGetValue(weaponId, out var ammoCount))
            {
                if (ammoCount >= value)
                {
                    _totalAmmoRepository[weaponId] -= value;
                    _clipAmmoRepository[weaponId] += value;
                    return value;
                }
                else
                {
                    var ammo = _totalAmmoRepository[weaponId];
                    _totalAmmoRepository[weaponId] = 0;
                    _clipAmmoRepository[weaponId] += ammo;
                    return ammo;
                }
            }

            Debug.LogWarning($"Ammo repository does not contain ammo for weapon ID: {weaponId}");
            return 0;
        }
        
        public void AddTotalAmmo(int weaponId, int ammoCount)
        {
            if (ammoCount <= 0)
                return;
            
            if (_totalAmmoRepository.ContainsKey(weaponId))
            {
                _totalAmmoRepository[weaponId] += ammoCount;
            }
        }

        public void RemoveClipAmmo(int weaponId, int ammoCount)
        {
            if (ammoCount <= 0)
                return;
            
            if (_clipAmmoRepository.ContainsKey(weaponId))
            {
                _clipAmmoRepository[weaponId] -= ammoCount;
            }
        }
        
        public int GetClipAmmoCount(int weaponId)
        {
            return _clipAmmoRepository[weaponId];
        }

        public int GetTotalAmmoCount(int weaponId)
        {
            return _totalAmmoRepository[weaponId];
        }
    }
}
