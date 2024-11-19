using UnityEngine;
using System.Threading.Tasks;
using Database.Services;
using System.Collections;

namespace Game.Authentication
{
    using Database.Data;
    using Database.Payload;
    using Game.UI;
    using Player;
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

        public void SetUI()
        {
            _mainMenuPlayerInfo.SetName(_playerDataSO.PlayerName);
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
                float progress = Mathf.Clamp01(operation.progress / 0.9f); // Normalize progress (0 to 1)

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

        #region Transaction
        public IEnumerator Register(RegisterPayload registerRequest)
        {
            AuthService authService = new();

            yield return StartCoroutine(authService.RegisterUser(registerRequest));
        }

        public IEnumerator Login(UserLoginPayload loginRequest)
        {
            AuthService authService = new();
            PlayerService playerService = new();

            yield return StartCoroutine(authService.Login(loginRequest));

            //SetPlayer Data
            yield return StartCoroutine(playerService.GetPlayerByLoginNameAsync(
                loginRequest.loginName,
                playerPayload =>
                {
                    PlayerData playerData = new();
                    _playerDataSO.SetPlayerDataSO(playerData.SetPlayerDataByPlayerPayload(playerPayload));
                    SetUI();
                },
                error =>
                {
                    Debug.LogError($"Failed to fetch player data: {error}");
                }
                ));

            //Make sure its disposed
            authService = null;
            playerService = null;
        }
        #endregion
    }
}
