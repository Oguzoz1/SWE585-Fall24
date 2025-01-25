using Game.Network;
using Game.Player;
using Mirror;
using UnityEngine;

namespace Game.Data
{
    //There should be Data Handler service where the client data is sent to the game server.
    public class PlayerDataHandler : NetworkBehaviour
    {
        [SerializeField] private PlayerDataSO _playerDataSO; 
        public PlayerData PlayerData { get; private set; }

        public delegate void PlayerDataReadyDelegate(PlayerData playerData);
        public event PlayerDataReadyDelegate OnPlayerDataReady;

      
        //Authentication happens when client makes a check call with their token
        //If the playerid within token matches with the playerData on the mirror server, mirror server grants authentication.
        //If it is not authenticated then the player is kicked out from the server.

        private bool _isAuthenticated = false;
        /// <summary>
        /// If True, then the player ID sent by the client to the game server is checked and confirmed by the backend server. 
        /// </summary>
        public bool IsAuthenticated { get { return _isAuthenticated; } }

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

        /// <summary>
        /// Server calls this on clients with isLocal flag. It requests client to prepare data and request server to receive it.
        /// </summary>
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


        /// <summary>
        /// Sets player data on the server and invokes OnPlayerDataReady with data provided by the client. 
        /// </summary>
        /// <param name="playerData"></param>
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
