using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

namespace Database.Client
{
    public class PlayerService : MonoBehaviour
    {
        private string apiUrl = "http://127.0.0.1:5071/api/player";

        [System.Serializable]
        public class Player
        {
            public long id;
            public string name;
        }


        public IEnumerator CreatePlayerAsync(Player newPlayer)
        {
            string jsonData = JsonUtility.ToJson(newPlayer);
            byte[] postData = Encoding.UTF8.GetBytes(jsonData);

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

        public IEnumerator GetPlayerAsync(long id)
        {
            using (UnityWebRequest request = UnityWebRequest.Get($"{apiUrl}/{id}"))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string jsonResult = request.downloadHandler.text;
                    Player fetchedPlayer = JsonUtility.FromJson<Player>(jsonResult);
                    Debug.Log($"Fetched Player: {fetchedPlayer.name}");
                }
                else
                {
                    Debug.LogError($"Error fetching player: {request.error}");
                }
            }
        }
    }
}
