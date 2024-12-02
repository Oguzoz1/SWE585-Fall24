using AeriaUtil.Pattern;
using UnityEngine;

namespace AeriaUtil.Systems.GlobalEvent
{
    public class GlobalEventSystem : Singleton<GlobalEventSystem>
    {
        [Header("Global Events")]
        [SerializeField] private GlobalEvent[] _globalEvents;


        [SerializeField] private GlobalEventListSO _globalEventList;
        public GlobalEventListSO GlobalEventList
        {
            get
            {
                if (_globalEventList == null)
                {
                    if (_globalEventList == null) DebugExt.LogErrorCheckObjExist(_globalEventList);
                    return _globalEventList;
                }
                return _globalEventList;
            }
        }
        public GlobalEvent[] GlobalEvents
        {
            get
            {
                if (_globalEvents == null || _globalEvents.Length == 0)
                {
                    _globalEvents = GlobalEventList.GlobalEvents;
                    return _globalEvents;
                }
                return _globalEvents;
            }
        }

        /// <summary>
        /// Increases trigger point for the specific event.
        /// </summary>
        /// <param name="eventName">name of the event.</param>
        /// <param name="pointsToIncrease">how many points it will be increased.</param>
        public void ProvokeTrigger(string eventName)
        {
            GlobalEvent e = GetEvent(eventName);
            e.IncreaseTriggerPoint();
        }
        /// <summary>
        /// Invokes the events bound to the provided eventName.
        /// </summary>
        /// <param name="eventName"></param>
        public void TriggerEvent(string eventName)
        {
            GlobalEvent e = GetEvent(eventName);
            e.Invoke();
        }
        /// <summary>
        /// Returns a GlobalEvent through provided string input.
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public GlobalEvent GetEvent(string eventName)
        {
            foreach (GlobalEvent e in GlobalEvents)
            {
                if (e.Name == eventName) return e;
            }
            return null;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            //If we want to see the content in the inspector before hand
            _globalEventList = ScriptableObjectFinder.FindScriptableObject<GlobalEventListSO>();

            //Validate the items.
            ValidateGlobalEvents();
        }

        private void ValidateGlobalEvents()
        {
            if (_globalEvents == null || _globalEvents.Length != GlobalEventList.GlobalEvents.Length)
            {
                _globalEvents = GlobalEventList.GlobalEvents;
            }
        }
#endif
    }
}
