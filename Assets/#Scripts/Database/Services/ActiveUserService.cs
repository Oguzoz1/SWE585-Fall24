using System.Collections;
using UnityEngine;

namespace Database.Services
{
    using UnityEngine.Networking;
    using Utility;

    public class ActiveUserService : IDatabaseService
    {
        private string apiUrl = AppUrls.API_ACTIVEUSER_URL;

        //Right now usercredId is used to determine the active users.
        public IEnumerator SendHeartbeat(int playerId)
        {
            Debug.Log("Sending heartbeat...");
            string token = UserProperties.USER_TOKEN;

            // Prepare the playerId payload
            byte[] postData = ApiUtility.PreparePostData(playerId);

            using (UnityWebRequest request = new UnityWebRequest($"{apiUrl}/heartbeat/{playerId}", "POST"))
            {
                request.uploadHandler = new UploadHandlerRaw(postData);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Authorization", $"Bearer {token}");

                // Send the request
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log($"Heartbeat sent successfully for player {playerId}");
                }
                else
                {
                    Debug.LogError($"Failed to send heartbeat for player {playerId}: {request.error}");
                    if (!string.IsNullOrEmpty(request.downloadHandler.text))
                    {
                        Debug.LogError($"Server Response: {request.downloadHandler.text}");
                    }
                }
            }
        }

        public IEnumerator RemovePlayer(int playerId)
        {
            Debug.Log("Removing player...");
            string url = $"{apiUrl}/remove/{playerId}";

            using (UnityWebRequest request = UnityWebRequest.Delete(url))
            {
                string token = UserProperties.USER_TOKEN; 
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
