using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace AeriaUtil.Systems.GameTemplate
{
    [CreateAssetMenu(fileName = "PlayerData",
menuName = "ScriptableObjects/GameManager/PlayerDataSO")]
    public class PlayerStatDataSO : StatDataSO
    {
        public string PlayerName
        {
            get
            {
                return _playerName;
            }
        }
        private string _playerName;

    }
}
