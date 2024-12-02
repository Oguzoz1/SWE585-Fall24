using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AeriaUtil.Systems.TimeManager
{
    public class DaySetter : MonoBehaviour, IDayTimeEvent
    {
        [SerializeField]
        private GameDayData[] _gameDayData;


        private void Start() => Init();

        #region Init
        private void Init()
        {
            TimeManager.Instance.TimeCounter
                .OnDayChange.Subscribe(HandleTimeEvents);
        }
        private void OnDisable()
        {
            TimeManager.Instance.TimeCounter
                .OnDayChange.Unsubscribe(HandleTimeEvents);
        }
        #endregion

        public void SetGameDay(GameDayData gameDayData)
        {
            //Set Timer Speed
            TimeManager.Instance.SetTimeSpeedMultiplier(gameDayData.TimeMultiplier);
        }

        public void SetGameDay()
        {
            Debug.Log("GAMEDAY SETTED");
            GameDayData currentDayData = FindDayData();

            SetGameDay(currentDayData);
        }
        private GameDayData FindDayData()
        {
            foreach (GameDayData gameDayData in _gameDayData)
            {
                if (TimeManager.Instance.GameDateTime.DayOfWeek == gameDayData.Day)
                {
                    return gameDayData;
                }
            }
            return null;
        }

        public void HandleTimeEvents()
        {
            SetGameDay();
        }
    }

   
}
