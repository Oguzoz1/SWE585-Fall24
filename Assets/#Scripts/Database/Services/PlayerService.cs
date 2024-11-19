using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

namespace Database.Services
{
    using Payload;
    using System;
    using Utility;

    public class PlayerService : IDatabaseService
    {
        private string apiUrl = AppUrls.API_PLAYER_URL;
        public IEnumerator CreatePlayerAsync(PlayerPayload newPlayer)
        {
            byte[] postData = ApiUtility.PreparePostData(newPlayer);

            using (UnityWebRequest request = new UnityWebRequest($"{apiUrl}/create", "POST"))
            {
                request.uploadHandler = new UploadHandlerRaw(postData);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log("Player created successfully!");
                    Debug.Log(request.downloadHandler.text);
                }
                else
                {
                    Debug.LogError($"Error creating player: {request.error}");
                }
            }
        }

        public IEnumerator GetPlayerByIdAsync(long id)
        {
            string token = UserProperties.USER_TOKEN;
            using (UnityWebRequest request = UnityWebRequest.Get($"{apiUrl}/{id}"))
            {
                request.SetRequestHeader("Authorization", $"Bearer {token}");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string jsonResult = request.downloadHandler.text;
                    PlayerPayload fetchedPlayer = JsonUtility.FromJson<PlayerPayload>(jsonResult);
                    Debug.Log($"Fetched Player: {fetchedPlayer.playerName}");
                }
                else
                {
                    Debug.LogError($"Error fetching player: {request.error}");
                }
            }
        }

        public IEnumerator GetPlayerByLoginNameAsync(string loginname, Action<PlayerPayload> onSuccess, Action<string> onError = null)
        {
            string token = UserProperties.USER_TOKEN;

            using (UnityWebRequest request = UnityWebRequest.Get($"{apiUrl}/loginname/{loginname}"))
            {
                request.SetRequestHeader("Authorization", $"Bearer {token}");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    try
                    {
                        string jsonResult = request.downloadHandler.text;
                        PlayerPayload response = JsonUtility.FromJson<PlayerPayload>(jsonResult);

                        if (response != null)
                        {
                            PlayerPayload playerPayload = response;
                            onSuccess?.Invoke(playerPayload); 
                        }
                        else
                        {
                            onError?.Invoke("Error: Unable to parse server response.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Parsing error: {ex.Message}");
                        onError?.Invoke("Error: Failed to parse response.");
                    }
                }
                else
                {
                    Debug.LogError($"Error: {request.error}");
                    onError?.Invoke($"Error: {request.error}");
                    if (!string.IsNullOrEmpty(request.downloadHandler.text))
                    {
                        Debug.LogError($"Server Response: {request.downloadHandler.text}");
                    }
                }
            }
        }

    }
}
