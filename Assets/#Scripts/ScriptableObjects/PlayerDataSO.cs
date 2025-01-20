using Database.Payload;
using Database.Utility;
using Game.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    [CreateAssetMenu(fileName = "PlayerDataSo", menuName ="ScriptableObjects/PlayerDataSo")]
    public class PlayerDataSO : ScriptableObject
    {
        public PlayerData PlayerData;

        public void SetPlayerDataSO(PlayerPayload payload)
        {
            PlayerData = PlayerData?.mapToPlayerData(payload) ?? new PlayerData().mapToPlayerData(payload);
        }
    }
}
