using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AeriaUtil.Systems.GameTemplate
{
    public abstract class AbstractPlayer : MonoBehaviour
    {
        public PlayerStatManager playerStats
        {
            get
            {
                if (_playerStats == null)
                {
                    if (TryGetComponent<PlayerStatManager>(out PlayerStatManager component))
                    {
                        _playerStats = component;
                    }
                    else DebugExt.LogErrorCheckObjExist(_playerStats);
                }
                return _playerStats;
            }
        }
        private PlayerStatManager _playerStats;
    }
}