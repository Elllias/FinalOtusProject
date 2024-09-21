using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Other
{
    public class HitEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;
        
        public async UniTask PlayEffect()
        {
            _particleSystem.Play();

            await UniTask.WaitWhile(()=>_particleSystem.isPlaying);
        }
    }
}