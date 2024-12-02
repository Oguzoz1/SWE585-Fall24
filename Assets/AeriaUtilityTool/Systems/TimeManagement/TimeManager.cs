using AeriaUtil.Pattern;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AeriaUtil.Systems.TimeManager
{
    /// <summary>
    /// Time Manager is a controller class for time-based functions. It includes getter and setter methods.
    /// </summary>
    [RequireComponent(typeof(TimeCounter))]
    public class TimeManager : Singleton<TimeManager>
    {
        public TimeCounter TimeCounter
        {
            get
            {
                if (_timeCounter == null)
                {
                    //Lazy Init
                    _timeCounter = GetComponent<TimeCounter>();
                }
                return _timeCounter;

            }
        }

        private TimeCounter _timeCounter;

        public string InGameTime { get { return TimeCounter.GetInGameTime(TimeCounter.GetGameDateTime()); } }
        public string InGameDay { get { return TimeCounter.GetInGameDay(); } }
        public DateTime GameDateTime { get { return TimeCounter.GetGameDateTime(); } }
        public DateTime InitialTime { get { return TimeCounter.InitialTime; } }

        /// <summary>
        /// Manipulates time elapse speed by multiplying given input.
        /// </summary>
        /// <param name="newInput"></param>
        public void SetTimeSpeedMultiplier(float newInput)
        {
            TimeCounter.SetTimeSpeedMultiplier(newInput);
        }

    }
}

