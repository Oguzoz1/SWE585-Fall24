using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AeriaUtil
{
    [RequireComponent(typeof(ITriggerLogic))]
    public class TriggerLogic : MonoBehaviour
    {
        [Header("Trigger Events")]
        [SerializeField] private TriggerEvent[] triggerEvents = null;

        private void OnTriggerEnter(Collider other)
        {
            foreach (TriggerEvent triggerEvent in triggerEvents)
            {
                if (triggerEvent.TriggerName == other.tag) triggerEvent.ExecuteEvents(other);
            }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            foreach (TriggerEvent triggerEvent in triggerEvents)
            {
                if (triggerEvent.TriggerName == other.tag) triggerEvent.ExecuteEvents(other);
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
            foreach (TriggerEvent triggerEvent in triggerEvents)
            {
                if (triggerEvent.TriggerName == collision.gameObject.tag) triggerEvent.ExecuteEvents(collision);
            }
        }
        public TriggerEvent GetEvent(string tag)
        {
            TriggerEvent tEvent = null;
            foreach (TriggerEvent triggerEvent in triggerEvents)
                tEvent = triggerEvent.TriggerName == tag ? triggerEvent : null;
            return tEvent;
        }
    }
}