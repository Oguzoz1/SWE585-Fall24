using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AeriaUtil.Systems.ObjectPlacement
{
    [Serializable]
    public class PlacementEvent : AbstractEvent
    {
        public UnityEvent UnityEvent;

        public override void Invoke()
        {
            base.Invoke();
            UnityEvent?.Invoke();
        }
    }
}