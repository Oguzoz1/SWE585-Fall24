using System.Collections;
using UnityEngine;

namespace Database.Services
{
    using UnityEngine.Networking;
    using Utility;

    public class ActiveUserService : IDatabaseService
    {
        private string apiUrl = AppUrls.API_ACTIVEUSER_URL;

        public IEnumerator ServerMovePlayerToInGame(int playerId)
        {
            Debug.Log("Moving player to in-game players...");

            yield return ApiUtility.SendRequestWithToken<string>($"{apiUrl}/player-to-game/{playerId}", "POST",
                onSuccess: (response) =>
                {
                    Debug.Log(response.message);
                    Debug.Log(response.data);

                },
                onError: (error) =>
                {
                    Debug.Log(error);
                });
        }

        public IEnumerator SendHeartbeat(int playerId)
        {
            Debug.Log("Sending heartbeat...");


            yield return ApiUtility.SendRequestWithToken<string>($"{apiUrl}/heartbeat/{playerId}", "POST",
                onSuccess: (response) =>
                {
                    Debug.Log(response.data);
                },
                onError: (error) =>
                {
                    Debug.LogError(error);
                });
        }
        public IEnumerator RemovePlayer(int playerId)
        {
            Debug.Log("Removing player...");
            string url = $"{apiUrl}/remove/{playerId}";

            using (UnityWebRequest request = UnityWebRequest.Delete(url))
            {
                string token = TokenManager.GetToken(); 
                if (!string.IsNullOrEmpty(token))
                {
                    request.SetRequestHeader("Authorization", $"Bearer {token}");
                }

                // Send the request
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log($"Player {playerId} removed successfully.");
                }
                else
                {
                    string errorMessage = request.downloadHandler != null
                        ? request.downloadHandler.text
                        : "Unknown error occurred";

                    Debug.LogError($"Failed to remove player {playerId}: {errorMessage}");
                }
            }
        }
    }
}
