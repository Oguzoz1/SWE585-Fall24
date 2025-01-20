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

            yield return ApiUtility.SendGetRequestWithToken<PlayerPayload>($"{apiUrl}/{id}",
                onSuccess: (response) =>
                {
                    Debug.Log($"Fetched Player: {response.data.playerName}");

                    //TODO
                    throw new NotImplementedException();
                },
                onError: (error) =>
                {
                    Debug.Log($"Fetching Failed: {error}");
                }
                );
        }

        public IEnumerator GetPlayerByLoginNameAsync(string loginname, Action<PlayerPayload> onSuccess, Action<string> onError = null)
        {

            yield return ApiUtility.SendGetRequestWithToken<PlayerPayload>($"{apiUrl}/loginname/{loginname}",
                onSuccess: (response) =>
                {
                    if (response != null || response.data != null)
                    {
                        Debug.Log($"Fetching Player Complete: {response.data.playerName}");
                        Debug.Log(response.data);
                        Debug.Log(response);
                        onSuccess?.Invoke(response.data);
                    }
                    else
                    {
                        onError?.Invoke("Error: Unable to parse server response.");
                    }
                },
                onError: (error) =>
                {
                    Debug.LogError($"Fetching faield: {error}");
                    onError?.Invoke($"Fetching faield: {error}");
                });
            
        }

    }
}
