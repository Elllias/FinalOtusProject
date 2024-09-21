using System.Collections.Generic;
using Core.Configs;
using Core.Other;
using UnityEngine;

namespace Core.Factories
{
    public class HitEffectFactory
    {
        private readonly HitEffectConfig _config;
        private readonly Transform _parent;
        private readonly Dictionary<LayerMask, Queue<HitEffect>> _hitEffectPool = new();

        public HitEffectFactory(HitEffectConfig config, Transform parent)
        {
            _config = config;
            _parent = parent;
        }

        public async void SpawnEffect(int layer, Vector3 position, Quaternion rotation)
        {
            var layerMask = LayerMask.GetMask(LayerMask.LayerToName(layer));

            if (!_hitEffectPool.TryGetValue(layerMask, out var pool))
            {
                pool = new Queue<HitEffect>();
                _hitEffectPool[layerMask] = pool;
            }

            HitEffect effect;

            if (pool.Count > 0)
            {
                effect = pool.Dequeue();
                effect.gameObject.SetActive(true);

                effect.transform.position = position;
                effect.transform.rotation = rotation;
            }
            else
            {
                var hitEffectPrefab = _config.GetEffect(layerMask) ?? _config.GetEffect(LayerMask.GetMask("Default"));
                
                if (hitEffectPrefab == null)
                {
                    Debug.LogError("Не удалось найти префаб эффекта.");
                    return;
                }

                effect = Object.Instantiate(hitEffectPrefab, position, rotation, _parent);
            }

            try
            {
                await effect.PlayEffect();
            }
            finally
            {
                effect.gameObject.SetActive(false);

                if (!_hitEffectPool.ContainsKey(layerMask))
                {
                    _hitEffectPool[layerMask] = new Queue<HitEffect>();
                }

                _hitEffectPool[layerMask].Enqueue(effect);
            }
        }
    }
}