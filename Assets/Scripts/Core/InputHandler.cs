using System;
using Core.Events;
using Modules.EventBusFeature;
using UnityEngine;

namespace Core
{
    public class InputHandler : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                EventBus.RaiseEvent(new ShootEvent());
            }
        }
    }
}