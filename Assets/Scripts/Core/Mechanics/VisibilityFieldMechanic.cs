using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Components;
using Core.Events;
using Modules.EventBusFeature;
using UnityEngine;

namespace Core.Mechanics
{
    public class VisibilityFieldMechanic
    {
        private readonly VisibilityFieldComponent _component;
        private readonly int _sourceId;

        private CancellationTokenSource _source;
        private bool _wasVisible;
        private DateTime _lastSeenTime;

        public VisibilityFieldMechanic(VisibilityFieldComponent component, int sourceId)
        {
            _component = component;
            _sourceId = sourceId;
        }

        public async void StartAsync()
        {
            _source = new CancellationTokenSource();
            
            while (!_source.IsCancellationRequested)
            {
                var isTargetVisible = IsTargetVisible();
                var isInMemoryTime = _wasVisible && (DateTime.Now - _lastSeenTime).TotalSeconds <= _component.GetMemoryTime();
                
                if (!_wasVisible && isTargetVisible)
                {
                    _wasVisible = true;
                    _lastSeenTime = DateTime.Now;
                    
                    EventBus.RaiseEvent(new PlayerFoundedEvent{SourceId = _sourceId});
                }
                else if (_wasVisible && !isTargetVisible && !isInMemoryTime)
                {
                    _wasVisible = false;
                    
                    EventBus.RaiseEvent(new PlayerLostEvent{SourceId = _sourceId});
                }

                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(0.5), _source.Token);
                }
                catch
                {
                    return;
                }
            }
            
            _source.Dispose();
            _source = null;
        }

        public bool IsTargetVisible()
        {
            var radius = _component.GetRadius();
            var targetPosition = _component.GetTarget().position;
            var ownerPosition = _component.GetVisionPoint().position;
            
            var intoDistance = Vector3.Distance(targetPosition, ownerPosition) <= radius;
            var isVisible = false;
            
            if (intoDistance)
            {
                var direction = targetPosition - ownerPosition;
                
                var layerMask = ~LayerMask.GetMask("Enemy");

                if (Physics.Raycast(ownerPosition, direction, out var hit, radius, layerMask))
                {
                    if (hit.transform == _component.GetTarget())
                    {
                        isVisible = true;
                    }
                }
            }
            
            return intoDistance && isVisible;
        }
        
        public void Stop()
        {
            _source?.Cancel();
        }
    }
}