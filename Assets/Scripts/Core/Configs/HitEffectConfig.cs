using System.Collections.Generic;
using Core.Data;
using UnityEngine;

namespace Core.Configs
{
    [CreateAssetMenu(fileName = "HitEffectConfig", menuName = "Config/Hit Effect Config", order = 0)]
    public class HitEffectConfig : ScriptableObject
    {
        [SerializeField] private List<HitEffectConfigData> _hitEffectConfigs;

        public HitEffect GetEffect(LayerMask mask)
        {
            foreach (var config in _hitEffectConfigs)
            {
                if (mask.value == (mask.value & config.Mask.value))
                {
                    return config.Effect;
                }
            }

            Debug.LogError($"No hit effect found for layer mask: {mask}");
            return null;
        }
    }
}