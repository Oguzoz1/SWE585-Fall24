using System;
using UnityEngine;
using UnityEngine.Events;

namespace AeriaUtil.Systems.GlobalEvent
{
    /// <summary>
    /// Class to provide an event dedicated to be triggered across all game.
    /// This class is a point-based trigger event. Invokes upon meeting the condition.
    /// </summary>
    [Serializable]
    public class GlobalEvent : AbstractEvent
    {
        public string Name { get { return _name; } }
        public int TriggerLimit { get { return _triggerLimit; } }
        public float TriggerLimitModifier { get { return _triggerLimitModifier; } }
        public int CurrentTriggerPoints { get { return _currentTriggerPoints; } }
        public int TriggerPointIncrementValue { get { return _triggerPointIncrementValue; } }

        [Header("Global Event Setting")]
        [SerializeField] private string _name;
        [SerializeField] private int _triggerPointIncrementValue = 1;
        [SerializeField] private int _triggerLimit;
        [SerializeField] private float _triggerLimitModifier = 1f;
        [SerializeField] private int _currentTriggerPoints;
        [SerializeField] private UnityEvent _unityEvent;

        /// <summary>
        /// Sets upper bound for the trigger point.
        /// </summary>
        private void SetTriggerLimit()
        {
            int newLimit = (int)(_triggerLimit * _triggerLimitModifier);
            _triggerLimit = newLimit;
        }
        /// <summary>
        /// Increases current trigger point by set increment value. 
        /// </summary>
        public void IncreaseTriggerPoint()
        {
            _currentTriggerPoints += _triggerPointIncrementValue;
            if (_currentTriggerPoints >= _triggerLimit)
            {
                Invoke();
                SetTriggerLimit();
            }
        }
        /// <summary>
        /// Increases current trigger point by a given value.
        /// </summary>
        /// <param name="point"></param>
        public void IncreaseTriggerPoint(int point)
        {
            _currentTriggerPoints += point;
            if (_currentTriggerPoints >= _triggerLimit)
            {
                Invoke();
                SetTriggerLimit();
            }
        }
        /// <summary>
        /// Resets content of current trigger point.
        /// </summary>
        private void ResetCurrentTriggerPoint()
        {
            _currentTriggerPoints = 0;
        }
        /// <summary>
        /// Invokes its event.
        /// </summary>
        public override void Invoke()
        {
            Debug.Log($"{Name} Event is triggered!");

            base.Invoke();
            _unityEvent?.Invoke();

            //Current Trigger Points must be reset everytime the method is called.
            ResetCurrentTriggerPoint();
        }
    }
}
