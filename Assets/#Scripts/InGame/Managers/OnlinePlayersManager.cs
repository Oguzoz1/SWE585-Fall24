using Game.Data;
using Mirror;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Network
{
    public class OnlinePlayersManager
    {
        private static OnlinePlayersManager _instance;
        public static OnlinePlayersManager Instance => _instance ??= new OnlinePlayersManager();

        //playerId to PlayerData
        private readonly Dictionary<int, PlayerData> _onlinePlayers = new();



        #region Server
        /// <summary>
        /// Adds a player to the online players list.
        /// Should be called by server-side logic when a player connects.
        /// </summary>
        /// <param name="connectionId">connectionId from the server networkbehaviour.</param>
        /// <param name="playerData">Player data to associate with the NetId.</param>
        [Server]
        public void ServerAddPlayer(int connectionId, PlayerData playerData)
        {
            if (_onlinePlayers.ContainsKey(connectionId))
            {
                Debug.LogWarning($"Player with NetId {connectionId} is already online. Updating player data.");
                _onlinePlayers[connectionId] = playerData;
                return;
            }
            _onlinePlayers.Add(connectionId, playerData);
            Debug.Log($"Player {playerData.PlayerName} added with ConnectionId {connectionId}.");
        }

        /// <summary>
        /// Removes a player from the online players list.
        /// Should be called by server-side logic when a player disconnects.
        /// </summary>
        /// <param name="connectionId">connectionId from the server networkbehaviour.</param>
        [Server]
        public bool ServerRemovePlayer(int connectionId)
        {
            if (_onlinePlayers.Remove(connectionId, out var playerData))
            {
                Debug.Log($"Player {playerData.PlayerName} removed with NetId {connectionId}.");
                return true;
            }
            else
            {
                Debug.LogWarning($"Attempted to remove non-existent player with NetId {connectionId}.");
                return false;
            }
        }
        /// <summary>
        /// Returns the designated player upon provided netId.
        /// </summary>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        [Server]
        public PlayerData ServerGetOnlinePlayer(int connectionId)
        {
            if (_onlinePlayers.TryGetValue(connectionId, out var playerData))
                return playerData;
            else
            {
                Debug.LogError($"Could not retrieve the player with Connection ID:{connectionId}");
                return null;
            }
        }

        /// <summary>
        /// Retrieves the list of online players. Server-only.
        /// </summary>
        /// <returns>List of PlayerData representing online players.</returns>
        [Server]
        public List<PlayerData> ServerGetOnlinePlayers()
        {
            return new List<PlayerData>(_onlinePlayers.Values);
        }
        #endregion
    }
}
