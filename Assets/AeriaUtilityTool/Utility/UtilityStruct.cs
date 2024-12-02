using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AeriaUtil.Systems
{
    [Serializable]
    public struct Index
    {
        public float Value
        {
            get
            {
                _value = Mathf.Clamp(_value, MinValue, MaxValue);
                return _value;
            }
        }
        [SerializeField] private float _value;

        public float MaxValue
        {
            get
            {
                return _maxValue;
            }
            set
            {
                _maxValue = value;
            }
        }
        [SerializeField] private float _maxValue;

        public float MinValue
        {
            get
            {
                return _minValue;
            }
            set
            {
                _minValue = value;
            }
        }
        [SerializeField] private float _minValue;

        public Index IncreaseIndexValue(float value)
        {
            _value += value;
            return this;
        }

        public Index DecreaseIndexValue(float value)
        {
            _value -= value;
            return this;
        }

        public Index SetIndexValue(float value)
        {
            _value = value;
            return this;
        }


    }
}
