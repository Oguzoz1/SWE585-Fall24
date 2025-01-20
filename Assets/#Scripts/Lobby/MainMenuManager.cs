using UnityEngine;
using Database.Services;
using System.Collections;

namespace Game.Authentication
{
    using Database.Payload;
    using Game.UI;
    using Player;
    using System;
    using UnityEngine.SceneManagement;

    public interface IAuthenticationManager
    {

    }

    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private PlayerDataSO _playerDataSO;
        public PlayerDataSO PlayerDataSO { get { return _playerDataSO; } }
        [SerializeField] private PlayerNameUI _mainMenuPlayerInfo;
        [SerializeField] private Animator _animator;
        [SerializeField] private ActiveUsersManager _ausManager;

        private AuthService authService = new AuthService();
        private PlayerService playerService = new PlayerService();

        public void SetUI()
        {
            _mainMenuPlayerInfo.SetName(_playerDataSO.PlayerData.PlayerName);
        }

        public void LoadScene(string sceneName)
        {
            StartCoroutine(LoadSceneAsync(sceneName));
        }

        private IEnumerator LoadSceneAsync(string sceneName)
        {
            // Show the loading screen
            _animator.Play("Loading");

            // Begin loading the scene
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            operation.allowSceneActivation = false; // Prevent auto-activation

            // Update the progress bar
            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / 0.9f); 

                // Update UI elements (optional)
                //if (progressText != null) progressText.text = $"{(int)(progress * 100)}%";

                // Allow scene activation when loading is complete
                if (operation.progress >= 0.9f)
                {
                    // Add a small delay or trigger scene activation when ready
                    operation.allowSceneActivation = true;
                }

                yield return null;
            }
        }
        ///This is set on Exit button in Lobby-UI
        public void Quit()
        {
            Application.Quit();
            //Remove Player from the lobbylist
            Debug.Log("REMOVE PLAYER CALL");
            StartCoroutine(_ausManager.RemovePlayer(_playerDataSO.PlayerData.PlayerId));
        }
        private void OnApplicationQuit()
        {
            //Remove Player from the lobbylist
            Debug.Log("REMOVE PLAYER CALL");
            StartCoroutine(_ausManager.RemovePlayer(_playerDataSO.PlayerData.PlayerId));
        }

        #region Transaction
        public IEnumerator Register(RegisterPayload registerRequest, Action onSuccess ,Action<string> onError = null)
        {

            yield return StartCoroutine(authService.ClientRegisterUser(registerRequest,
                success =>
                {
                    if (success)
                    {
                        //Handle Success
                        onSuccess?.Invoke();
                    }
                    else
                    {
                        //Handle Failure
                        onError?.Invoke("Something went wrong!");
                    }
                }
                ));
        }



        public IEnumerator Login(UserLoginPayload loginRequest, Action onSuccess, Action<string> onError = null)
        {

            yield return StartCoroutine(authService.ClientLogin(loginRequest, onError));

            //SetPlayer Data
            yield return StartCoroutine(playerService.GetPlayerByLoginNameAsync(
                loginRequest.loginName,
                playerPayload =>
                {
                    //Set PlayerData
                    _playerDataSO.SetPlayerDataSO(playerPayload);

                    //Set Player UI in MainMenu
                    SetUI();

                    //Trigger Event for successful loggedin
                    onSuccess?.Invoke();

                    //Start Sending Heartbeat
                    StartCoroutine(_ausManager.SendHeartbeatRoutine(_playerDataSO.PlayerData.PlayerId));
                },
                error =>
                {
                    Debug.LogError($"Failed to fetch player data: {error}");
                }
                ));
        }

        #endregion
    }
}
