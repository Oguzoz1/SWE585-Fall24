using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AeriaUtil
{
    //Create a script to handle trigger and subscribe to this logic.

    public interface ITriggerLogic
    {
        /// <summary>
        /// Use this to sub to events. Ex: TriggerLogic.GetEvent(string eventName).OnTriggerEntry += HandleTrigger;
        /// </summary>
        public void SubscribeEvents();
        /// <summary>
        /// Use this to UNSUB to events. Ex: TriggerLogic.GetEvent(string eventName).OnTriggerEntry -= HandleTrigger;
        /// </summary>
        public void UnsubscribeEvents();
    }
    public interface ITrigger2D : ITriggerLogic
    {
        /// <summary>
        /// Use this to handle event function for a 2D object.
        /// </summary>
        /// <param name="collider"></param>
        public void HandleTrigger(Collider2D collider);

    }
    public interface ITrigger3D : ITriggerLogic
    {
        /// <summary>
        /// Use this to handle event function for a 3D object.
        /// </summary>
        /// <param name="collider"></param>
        public void HandleTrigger(Collider collider);
    }
}