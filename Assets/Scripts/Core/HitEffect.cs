using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core
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