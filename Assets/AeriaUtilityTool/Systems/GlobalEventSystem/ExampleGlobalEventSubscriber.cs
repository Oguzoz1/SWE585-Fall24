using AeriaUtil.Systems.GlobalEvent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AeriaUtil.Systems.GlobalEvent
{
    public class ExampleGlobalEventSubscriber : MonoBehaviour, IGlobalEvent
    {
        //Hold reference to event to not to access it everytime from the singleton.
        private GlobalEvent testEvent;
        private void Awake()
        {
            //Injection through singleton.
            testEvent = GlobalEventSystem.Instance.GetEvent("testevent");
            testEvent.Subscribe(HandleGlobalEvents);
        }

        private void OnDisable()
        {
            testEvent.Unsubscribe(HandleGlobalEvents);

        }
        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                GlobalEventSystem.Instance.TriggerEvent("testevent");
            }
        }


        public void HandleGlobalEvents()
        {
            Debug.Log("TESTEVENT IS HANDLED");
        }
    }

}
