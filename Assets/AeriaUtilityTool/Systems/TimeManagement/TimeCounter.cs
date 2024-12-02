using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace AeriaUtil.Systems.TimeManager
{
    /// <summary>
    /// Responsible of In-Game time flow. 
    /// Time elapse speed can be modifiable.
    /// </summary>
    public class TimeCounter : MonoBehaviour
    {

        [SerializeField] 
        private float _gameTimeMultiplier = 20000.0f; //real life time mulitipler

        private float _elapsedTimeInSeconds;
        private DateTime _initalTime;             //session start time 
        private DateTime _currentTime;            //updated time 
        private int _lastDay = -1;                // Last day of the week

        //DI
        public TimeEvent OnDayChange
        {
            get
            {
                _onDayChange = _onDayChange == null ? new TimeEvent() : _onDayChange;
                return _onDayChange;
            }
        }
        [SerializeField] private TimeEvent _onDayChange;

        private void Awake()
        {
            //inital time = last saved current time
        }
        public void Start() //first time inittiated
        {
            // Set the initial time to Monday 10 am of the current week
            _initalTime = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek).AddDays(1).AddHours(10);
            Debug.Log("Initial Time Day Start:" + _initalTime.DayOfWeek);
        }

        public void Update()
        {
            TimeElapses();
        }


        private void TimeElapses()
        {
            float elapsedSecondsForDay = Time.deltaTime * _gameTimeMultiplier;
            _elapsedTimeInSeconds += elapsedSecondsForDay;
            _currentTime = _initalTime.AddSeconds(_elapsedTimeInSeconds);

            if (_lastDay != (int)_currentTime.DayOfWeek)
            {

                OnDayChange.Invoke();
                _lastDay = (int)_currentTime.DayOfWeek;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentTime"></param>
        /// <returns>Current Game Day</returns>
        public string GetInGameDay(DateTime currentTime)
        {
            return currentTime.DayOfWeek.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Current Game Day</returns>
        public string GetInGameDay()
        {
            return _currentTime.DayOfWeek.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentTime"></param>
        /// <returns>Current Game Date</returns>
        public string GetInGameTime(DateTime currentTime)
        {

            return currentTime.ToString("HH:mm");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Current DateTime for In-Game time.</returns>
        public DateTime GetGameDateTime()
        {
            return _currentTime;
        }
        /// <summary>
        /// Set new value for Game Time Multiplier.
        /// </summary>
        /// <param name="newInput"></param>
        public void SetTimeSpeedMultiplier(float newInput)
        {
            _gameTimeMultiplier = newInput;
        }
        public DateTime InitialTime => _initalTime;
    }

}

