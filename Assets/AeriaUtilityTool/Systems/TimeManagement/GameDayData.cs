using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AeriaUtil.Systems.TimeManager
{
    [Serializable]
    public class GameDayData
    {
        public DayOfWeek Day;
        public float TimeMultiplier = 20000f;
    }

}

