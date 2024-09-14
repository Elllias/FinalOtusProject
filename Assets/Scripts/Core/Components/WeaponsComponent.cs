using System;
using System.Collections.Generic;
using Core.Data;
using UnityEngine;

namespace Core.Components
{
    [Serializable]
    public class WeaponsComponent
    {
        [SerializeField] private List<WeaponData> _weapons;

        public List<WeaponData> GetWeaponsData()
        {
            return _weapons;
        }
    }
}