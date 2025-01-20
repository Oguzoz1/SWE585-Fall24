using Game.Data;
using Mirror;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Network
{
    public class OnlinePlayersManager : NetworkBehaviour
    {
        private static OnlinePlayersManager _instance;
        public static OnlinePlayersManager Instance => _instance ??= new OnlinePlayersManager();

        //NetId to playerId
        private readonly Dictionary<uint, PlayerData> _onlinePlayers = new();

        #region Server
        /// <summary>
        /// Adds a player to the online players list.
        /// Should be called by server-side logic when a player connects.
        /// </summary>
        /// <param name="netId">NetId of the player.</param>
        /// <param name="playerData">Player data to associate with the NetId.</param>
        [Server]
        public void AddPlayer(uint netId, PlayerData playerData)
        {
            if (_onlinePlayers.ContainsKey(netId))
            {
                Debug.LogWarning($"Player with NetId {netId} is already online. Updating player data.");
                _onlinePlayers[netId] = playerData;
                return;
            }
            _onlinePlayers.Add(netId, playerData);
            Debug.Log($"Player {playerData.PlayerName} added with NetId {netId}.");
        }

        /// <summary>
        /// Removes a player from the online players list.
        /// Should be called by server-side logic when a player disconnects.
        /// </summary>
        /// <param name="netId">NetId of the player to remove.</param>
        [Server]
        public void RemovePlayer(uint netId)
        {
            if (_onlinePlayers.Remove(netId, out var playerData))
            {
                Debug.Log($"Player {playerData.PlayerName} removed with NetId {netId}.");
            }
            else
            {
                Debug.LogWarning($"Attempted to remove non-existent player with NetId {netId}.");
            }
        }
        /// <summary>
        /// Returns the designated player upon provided netId.
        /// </summary>
        /// <param name="netId"></param>
        /// <returns></returns>
        [Server]
        public PlayerData GetOnlinePlayer(uint netId)
        {
            _onlinePlayers.TryGetValue(netId, out var playerData);
            return playerData;
        }

        /// <summary>
        /// Retrieves the list of online players. Server-only.
        /// </summary>
        /// <returns>List of PlayerData representing online players.</returns>
        [Server]
        public List<PlayerData> GetOnlinePlayers()
        {
            return new List<PlayerData>(_onlinePlayers.Values);
        }

        /// <summary>
        /// Handles client requests to get online player data.
        /// </summary>
        /// <param name="conn">Client connection requesting data.</param>
        [Server]
        public void HandlePlayerDataRequest(NetworkConnectionToClient conn)
        {
            if (!_onlinePlayers.ContainsKey(conn.identity.netId))
            {
                Debug.LogWarning($"Client {conn.connectionId} is not recognized as an online player.");
                return;
            }

            // Example: Send online player data to the requesting client
            TargetSendPlayerData(conn, new List<PlayerData>(_onlinePlayers.Values));
        }

        [TargetRpc]
        private void TargetSendPlayerData(NetworkConnection conn, List<PlayerData> playerDataList)
        {
            // Process received data on the client side
            Debug.Log($"Received {playerDataList.Count} online players from the server.");
        }
        #endregion

        #region Client
        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();

        }
        public override void OnStopLocalPlayer()
        {
            base.OnStopLocalPlayer();

        }
        #endregion


    }
}
