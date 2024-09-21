using System;
using Core.Other;
using UnityEngine;

namespace Core.Data
{
    [Serializable]
    public class HitSetting
    {
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private HitEffect _hitEffect;
        
        public int GetLayerMask()
        {
            return _layerMask;
        }
        
        public HitEffect GetHitEffect()
        {
            return _hitEffect;
        }
    }
}