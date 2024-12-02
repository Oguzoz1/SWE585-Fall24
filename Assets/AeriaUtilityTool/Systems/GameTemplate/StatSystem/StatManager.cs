using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AeriaUtil.Systems.GameTemplate
{
    public class StatManager : MonoBehaviour
    {
        public StatDataSO StatDataSO
        {
            get
            {
                if (_statDataSO == null)
                {
                    if (_statDataSO == null) DebugExt.LogErrorCheckObjExist(_statDataSO);
                    return _statDataSO;
                }
                return _statDataSO;
            }
            set
            {
                _statDataSO = value;
            }
        }
        [SerializeField] private StatDataSO _statDataSO;

        public Stat GetStat(string statID)
        {
            if (StatDataSO.StatsDictionary.ContainsKey(statID))
            {

                return StatDataSO.StatsDictionary[statID];
            }
            else
            {
                Debug.LogError($"'{statID}' DOES NOT EXIST!");
                return null;
            }
        }
    }

}
