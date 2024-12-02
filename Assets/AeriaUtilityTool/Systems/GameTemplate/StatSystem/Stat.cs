using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AeriaUtil.Systems.GameTemplate
{
    [Serializable]
    public class Stat
    {
        /// <summary>
        /// Each player stat includes set of indices. They could support multiple float values with names.
        /// </summary>
        [SerializedDictionary("Stat Index Name", "Stat Index Value"), SerializeField]
        private SerializedDictionary<string, Index> _statIndexMap = new SerializedDictionary<string, Index>();

        /// <summary>
        /// Gets stat value. Use this, if you are going to change multiple values.
        /// </summary>
        /// <param name="statIndexName"></param>
        /// <returns></returns>
        public Index GetStatIndex(string statIndexName)
        {
            if (_statIndexMap.ContainsKey(statIndexName))
                return _statIndexMap[statIndexName];
            else
            {
                Debug.LogError($"{statIndexName} DOES NOT EXIST!");
                return new Index();
            }
        }
        /// <summary>
        /// Set Index Value of the stat.
        /// </summary>
        /// <param name="statIndexName"></param>
        /// <param name="desiredValue"></param>
        /// <returns>Return the changed index</returns>
        public Index SetStatIndexValue(string statIndexName, float desiredValue)
        {
            //Set the new Index
            Index desiredIndex = GetStatIndex(statIndexName);
            desiredIndex.SetIndexValue(desiredValue);

            //List it in Dictionary
            SetDictionaryValue(statIndexName, desiredIndex);

            return desiredIndex;
        }

        public Index IncreaseStatValue(string statIndexName, float desiredIncrease)
        {
            //Set the new Index
            Index desiredIndex = GetStatIndex(statIndexName);
            desiredIndex.IncreaseIndexValue(desiredIncrease);

            //List it in Dictionary
            SetDictionaryValue(statIndexName, desiredIndex);

            return desiredIndex;
        }

        public Index DecreaseStatValue(string statIndexName, float desiredDecrease)
        {
            //Set the new Index
            Index desiredIndex = GetStatIndex(statIndexName);
            desiredIndex.DecreaseIndexValue(desiredDecrease);

            SetDictionaryValue(statIndexName, desiredIndex);

            return desiredIndex;
        }

        private void SetDictionaryValue(string keyID, Index value)
        {
            _statIndexMap[keyID] = value;
        }
    }

}
