using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AeriaUtil.Systems.Interact
{
    public abstract class InteractItem
    {
        public InteractEvent InteractEvent;
    }
    public class InteractEvent : AbstractEvent
    {
        [SerializeField] private UnityEvent _unityEvent;

        public override void Invoke()
        {
            base.Invoke();
            _unityEvent?.Invoke();
        }
    }
}
