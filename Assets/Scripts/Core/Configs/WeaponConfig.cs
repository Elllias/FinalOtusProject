using Unity.VisualScripting;
using UnityEngine;

namespace Core.Configs
{
    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "Config/WeaponConfig", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        public float Range;
        public bool IsUnlimited;
        public int MaxAmmoCount;
        public float ShootCooldown;
        public float ShootDuration;
        public float RecoilForce;
        public float ReloadingTime;
        public Vector3 ShootEffectPoint;
        public AudioClip ShootSound;
        public AudioClip DrySound;
        public AudioClip ReloadSound;
        public bool IsAutomatic;
    }
}