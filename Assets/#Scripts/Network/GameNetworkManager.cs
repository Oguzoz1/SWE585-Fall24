using Database;
using Database.Services;
using Game.Data;
using Game.Player;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Network
{
    public class GameNetworkManager : NetworkManager
    {

#if UNITY_SERVER
        ActiveUserService _activeUserService = new();
#endif

        public void LaunchGame()
        {
            StartClient();
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            //Check if the client is an authorized client else kick the client. If token does not exist or if the token expired or the response is bad
            //Then force disconnect the client.

        }
        public override void OnClientDisconnect()
        {
            base.OnClientDisconnect();
        }
        public override void OnServerSceneChanged(string newSceneName)
        {
            // Since we name our maps with Scene_Map, we compare.
            if (SceneManager.GetActiveScene().name.StartsWith("MainGame"))
            {

            }
        }
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            base.OnServerAddPlayer(conn);

#if UNITY_SERVER
            GameObject playerObject = conn.identity.gameObject;
            PlayerDataHandler playerDataHandler = playerObject.GetComponent<PlayerDataHandler>();
            if (playerDataHandler != null)
            {
                Debug.Log("PlayerDataHandler is found. Waiting for player data...");

                // Subscribe to the event to wait for player data readiness
                playerDataHandler.OnPlayerDataReady += playerData =>
                {
                    Debug.Log($"Player data is ready! Proceeding with backend call for player: {playerData.PlayerName}");

                    // Proceed with backend call
                    StartCoroutine(_activeUserService.ServerMovePlayerToInGame(playerData.PlayerId));
                };

                // Initiate the player data send
                playerDataHandler.RpcSendPlayerDataToServer();
            }
            else
            {
                Debug.LogError("PlayerDataHandler component not found on the spawned player object.");
            }
#endif
        }
        public override void OnServerConnect(NetworkConnectionToClient conn)
        {
            base.OnServerConnect(conn);

        }
        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            base.OnServerDisconnect(conn);

#if UNITY_SERVER
            Debug.Log($"Handling disconnection for connection ID {conn.connectionId}");

            if (OnlinePlayersManager.Instance == null)
            {
                Debug.LogError("OnlinePlayersManager.Instance is null. Cannot proceed.");
                return;
            }

            PlayerData playerToRemove = OnlinePlayersManager.Instance.ServerGetOnlinePlayer(conn.connectionId);
            if (playerToRemove == null)
            {
                Debug.LogError($"Player with connection ID {conn.connectionId} not found in OnlinePlayersManager.");
                return;
            }

            Debug.Log($"Player found: {playerToRemove.PlayerName}, removing from backend and OnlinePlayersManager.");
            StartCoroutine(_activeUserService.RemovePlayer(playerToRemove.PlayerId));
            OnlinePlayersManager.Instance.ServerRemovePlayer(conn.connectionId);
#endif
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            Debug.Log($"Server started on address {networkAddress} and port {transport.ServerActive()}");

#if UNITY_SERVER
            //Check Connection to database as Server
            ServerDatabaseConfig config = new ServerDatabaseConfig();
            AuthService authService = new AuthService();
            StartCoroutine(authService.ServerCheckServer());

#endif

        }

    }
}
