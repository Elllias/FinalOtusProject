using System;
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