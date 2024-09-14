using System;
using UnityEngine;

namespace Core.Components
{
    [Serializable]
    public class StepAudioComponent
    {
        public AudioSource Source;
        public float SoundCooldown;
    }
}