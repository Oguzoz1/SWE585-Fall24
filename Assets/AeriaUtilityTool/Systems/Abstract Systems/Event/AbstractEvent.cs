using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AeriaUtil.Systems
{
    public abstract class AbstractEvent
    {
        public delegate void Event();
        protected Event _event;

        public virtual void Subscribe(Event e)
        {
            _event += e;
        }

        public virtual void Unsubscribe(Event e)
        {
            _event -= e;
        }
        public virtual void Invoke()
        {
            _event?.Invoke();
        }
    }
}

