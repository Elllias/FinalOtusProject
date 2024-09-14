using System;
using Core.Configs;
using UnityEngine;

namespace Core.Data
{
    [Serializable]
    public struct WeaponData
    {
        public int Id;
        public GameObject WeaponObject;
        public WeaponConfig Config;
        public int ClipAmmoCount;
        public int TotalAmmoCount;
    }
}