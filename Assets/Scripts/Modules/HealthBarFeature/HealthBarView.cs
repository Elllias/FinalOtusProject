using Modules.SliderFeature;
using UnityEngine;

namespace Modules.HealthBarFeature
{
    public class HealthBarView : MonoBehaviour
    {
        [SerializeField] private Slider _fillRect;
        
        public void SetSliderValue(float value)
        {
            _fillRect.SetSliderValue(value);
        }
    }
}