using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace AeriaUtil.Systems.TimeManager
{
    /// <summary>
    /// This class is provides an event, specifically designed to be used in timed events.
    /// </summary>
    [Serializable]
    public class TimeEvent : AbstractEvent
    {

        public UnityEvent UnityEvent;
        /// <summary>
        /// Timed Coroutine must be executed from a monobehaviour since
        /// the method is for coroutines.
        /// </summary>
        /// <param name="timer"></param>
        /// <returns></returns>
        private IEnumerator ExecuteCountdownEvent(float timer)
        {
            yield return new WaitForSeconds(timer);
            Invoke();
        }

        public override void Invoke()
        {
            base.Invoke();
            UnityEvent?.Invoke();
        }
    }
}
