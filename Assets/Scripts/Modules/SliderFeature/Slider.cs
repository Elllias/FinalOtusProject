using UnityEngine;

namespace Modules.SliderFeature
{
    public class Slider : MonoBehaviour
    {
        [SerializeField] private RectTransform _fillRect;
        
        public void SetSliderValue(float value)
        {
            _fillRect.anchorMax = new Vector2(value, 1);
            _fillRect.anchorMin = new Vector2(0, 1);
        }
    }
}