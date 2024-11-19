using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Michsky.UI.Shift
{
    public class TimedEvent : MonoBehaviour
    {
        [Header("Timing (seconds)")]
        public float timer = 4;
        public bool enableAtStart;

        [Header("Timer Event")]
        public UnityEvent timerAction;

        [Header("3D Viewer")]
        public GameObject viewer;

        void Start()
        {
            if(enableAtStart == true)
            {
                StartCoroutine("TimedEventStart");
            }
        }

        IEnumerator TimedEventStart()
        {
            yield return new WaitForSeconds(timer);
            timerAction.Invoke();
        }

        public void StartIEnumerator ()
        {
            StartCoroutine("TimedEventStart");
        }

        public void StopIEnumerator ()
        {
            StopCoroutine("TimedEventStart");
        }

        public void TurnOn3DViewer()
        {
            viewer.SetActive(true);
        }
    }
}
