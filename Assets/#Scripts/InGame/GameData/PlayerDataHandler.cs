using Game.Network;
using Game.Player;
using Mirror;
using UnityEngine;

namespace Game.Data
{
    public class PlayerDataHandler : NetworkBehaviour
    {
        [SerializeField] private PlayerDataSO _playerDataSO;  // Ensure this is assigned in the Inspector
        public PlayerData PlayerData { get; private set; }

        private void Start()
        {
            // Log a warning if _playerDataSO is not assigned
            if (_playerDataSO == null)
            {
                Debug.LogError("PLAYER DATA SO IS NULL!");
            }

            if (isLocalPlayer && _playerDataSO != null)
            {
                PlayerData = _playerDataSO.PlayerData;

                // Only call SendPlayerData() after PlayerData is fully initialized
                if (PlayerData != null)
                {
                    SendPlayerData();
                }
                else
                {
                    Debug.LogError("PlayerData is null. Cannot send data.");
                }
            }
        }

        // Method to send data from client to server
        public PlayerData SendPlayerData()
        {
            if (isLocalPlayer || isServerOnly)
            {
                Debug.Log("Initiating sending data to player");

                // Check if PlayerDataSO is null
                if (_playerDataSO == null)
                {
                    Debug.LogError("Player SO is NULL!");
                    return null;
                }

                // Ensure PlayerData is initialized
                PlayerData ??= _playerDataSO.PlayerData;

                // Check if PlayerData is null
                if (PlayerData == null)
                {
                    Debug.LogError("PlayerData is null and cannot be sent.");
                    return null;
                }

                // Check if connectionToClient is null
                if (connectionToClient == null)
                {
                    Debug.LogError("connectionToClient is null!");
                    return null;
                }

                // Check if connectionToClient.identity is null
                if (connectionToClient.identity == null)
                {
                    Debug.LogError("connectionToClient.identity is null!");
                    return null;
                }

                // If everything is valid, send data to server
                Debug.Log($"Sending player data to server: {PlayerData.PlayerName}, playerId: {PlayerData.PlayerId}");
                CmdSendPlayerData(connectionToClient.identity.netId, PlayerData);

                return PlayerData;
            }
            else
            {
                Debug.LogError("Only the local player can send data to the server.");
                return null;
            }
        }

        [Command]
        private void CmdSendPlayerData(uint netId, PlayerData playerData)
        {
            // Store player data in OnlinePlayersManager
            OnlinePlayersManager.Instance.AddPlayer(netId, playerData);

            Debug.Log($"Received player data from client: {PlayerData.PlayerName}, playerId: {PlayerData.PlayerId}");
        }
    }
}
