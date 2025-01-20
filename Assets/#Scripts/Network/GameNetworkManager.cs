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
                Debug.Log("PlayerDataHandler is found now will try to send data");
                // Set the PlayerData on the server

                //PlayerData playerData = playerDataHandler.SendPlayerData();

                //StartCoroutine(_activeUserService.ServerMovePlayerToInGame(playerData.PlayerId));
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
            //Player Removal on disconnect
            uint playerToRemoveId = conn.identity.netId;
            PlayerData playerToRemove = OnlinePlayersManager
                .Instance.GetOnlinePlayer(playerToRemoveId);

            StartCoroutine(_activeUserService.RemovePlayer(playerToRemove.PlayerId));
            OnlinePlayersManager.Instance.RemovePlayer(playerToRemoveId);
#endif
        }
        public override void OnStartServer()
        {
            base.OnStartServer();
            Debug.Log($"Server started on address {networkAddress} and port {transport.ServerActive()}");

#if UNITY_SERVER
            //Connect To Database as Server
            ServerDatabaseConfig config = new ServerDatabaseConfig();
            //AuthService authService = new AuthService();
            //StartCoroutine(authService.ServerCheckServer());

#endif

        }

    }
}
