using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AeriaUtil
{


    [System.Serializable]
    public class TriggerEvent
    {
        [Header("Trigger Attributes")]
        [SerializeField] private string triggerName;
        [Tooltip("If its required to be dragged and drop use this.")]
        [SerializeField] private UnityEvent unityEvent;

        public string TriggerName { get => triggerName; private set => triggerName = value; }
        public event Action<Collision> OnCollisionEntry;
        public event Action<Collider> OnTriggerEntry;
        public event Action<Collider2D> OnTriggerEntry2D;

        public void ExecuteEvents(Collision collision)
        {
            unityEvent?.Invoke();
            OnCollisionEntry?.Invoke(collision);
        }
        public void ExecuteEvents(Collider2D collision)
        {
            unityEvent?.Invoke();
            OnTriggerEntry2D?.Invoke(collision);
        }
        public void ExecuteEvents(Collider collider)
        {
            unityEvent?.Invoke();
            OnTriggerEntry?.Invoke(collider);
        }
    }
}