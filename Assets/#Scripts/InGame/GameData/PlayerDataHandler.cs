using Game.Network;
using Game.Player;
using Mirror;
using UnityEngine;

namespace Game.Data
{
    public class PlayerDataHandler : NetworkBehaviour
    {
        [SerializeField] private PlayerDataSO _playerDataSO; 
        public PlayerData PlayerData { get; private set; }

        public delegate void PlayerDataReadyDelegate(PlayerData playerData);
        public event PlayerDataReadyDelegate OnPlayerDataReady;

        //Place an authenticated flag here.
        //Authentication happens when client makes a check call with their token
        //If the playerid within token matches with the playerData on the mirror server, mirror server grants authentication.
        //If it is not authenticated then the player is kicked out from the server.

        private void Awake()
        {
            if (isLocalPlayer)
            {
                //Check if playerData is set
                if (_playerDataSO == null)
                {
                    Debug.LogError("PLAYER DATA SO IS NULL!");
                }

                if (isLocalPlayer && _playerDataSO != null)
                {
                    PlayerData = _playerDataSO.PlayerData;

                    if (PlayerData != null)
                    {
                        Debug.Log("Player Data is set!");
                    }
                    else
                    {
                        Debug.LogError("PlayerData is null. Cannot send data.");
                    }
                }
            }
        }

        [ClientRpc]
        public void RpcSendPlayerDataToServer()
        {
            if (isLocalPlayer)
            {

                Debug.Log("Initiating sending data to player");

                // Check if PlayerDataSO is null
                if (_playerDataSO == null)
                {
                    Debug.LogError("Player SO is NULL!");
                    return;
                }

                // Ensure PlayerData is initialized
                PlayerData ??= _playerDataSO.PlayerData;

                // Check if PlayerData is null
                if (PlayerData == null)
                {
                    Debug.LogError("PlayerData is null and cannot be sent.");
                    return;
                }
                // If everything is valid, send data to server
                Debug.Log($"Sending player data to server: {PlayerData.PlayerName}, playerId: {PlayerData.PlayerId}");
                CmdSetPlayerData(PlayerData);

                return;
            }
            else
            {
                Debug.LogError("Only the local player can send data to the server.");
                return;
            }
        }

        //TODO: Create a player object. Decide whether or not to save the PlayerData paired with Player Object => Key: playerId, Value: Player
        [Command]
        private void CmdSetPlayerData(PlayerData playerData)
        {
            //Validate Client-Side Player Data:
            if (playerData != null)
                Debug.Log($"Received player data from client: {playerData.PlayerName}, playerId: {playerData.PlayerId} with connection Id: {this.connectionToClient.connectionId}");
            else Debug.LogError($"Player Data Input is NULL!");

            //Set client data on the server.
            PlayerData = playerData;

            if (!string.IsNullOrEmpty(PlayerData.PlayerName))
            {
                Debug.Log($"PlayerData is valid: {PlayerData.PlayerName}, {PlayerData.PlayerId}");

                // Notify listeners that the data is ready
                OnPlayerDataReady?.Invoke(PlayerData);
            }
            else
            {
                Debug.LogError("PlayerData is invalid!");
            }

            //Add Player data on the server.
            OnlinePlayersManager.Instance.ServerAddPlayer(this.connectionToClient.connectionId, playerData);

        }
    }
}
