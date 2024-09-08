using Modules.SliderFeature;
using UnityEngine;

namespace Modules.HealthBarFeature
{
    public class HealthBarView : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        
        public void SetSliderValue(float value)
        {
            _slider.SetSliderValue(value);
        }
    }
}