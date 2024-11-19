using Database.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    [CreateAssetMenu(fileName = "PlayerDataSo", menuName ="ScriptableObjects/PlayerDataSo")]
    public class PlayerDataSO : ScriptableObject
    {
        public int PlayerId;
        public int UserCredentialsId;
        public string PlayerName;
        public string LoginName;

        public void SetPlayerDataSO(PlayerData playerData)
        {
            PlayerId = playerData.PlayerId;
            UserCredentialsId = playerData.UserCredentialsId;
            PlayerName = playerData.PlayerName;
            LoginName = playerData.LoginName;
        }
    }
}
