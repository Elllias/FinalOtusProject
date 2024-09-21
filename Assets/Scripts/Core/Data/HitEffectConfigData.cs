using System;
using Core.Other;
using UnityEngine;

namespace Core.Data
{
    [Serializable]
    public class HitEffectConfigData
    {
        public LayerMask Mask;
        public HitEffect Effect;
    }
}