using Core.Components;
using UnityEngine;

namespace Core.Mechanics
{
    public class RotateThroughInputMechanic
    {
        private const float MAX_VERTICAL_OFFSET = 85f;
        
        private readonly RotateComponent _rotateComponent;
        private readonly float _mouseSensitivity;
        
        private float _xRotation;
        private float _yRotation;

        public RotateThroughInputMechanic(RotateComponent rotateComponent, float mouseSensitivity)
        {
            _rotateComponent = rotateComponent;
            _mouseSensitivity = mouseSensitivity;
            
            var currentRotation = rotateComponent.GetCurrentRotation();
            _xRotation = currentRotation.x;
            _yRotation = currentRotation.y;
        }
        
        public void Update()
        {
            if (!Application.isFocused) return;
            
            _yRotation += Input.GetAxis("Mouse X") * _mouseSensitivity;
            _xRotation -= Input.GetAxis("Mouse Y") * _mouseSensitivity;

            _xRotation = Mathf.Clamp(_xRotation, -MAX_VERTICAL_OFFSET, MAX_VERTICAL_OFFSET);
            
            _rotateComponent.Rotate(Quaternion.Euler(_xRotation, _yRotation, 0));
        }
    }
}