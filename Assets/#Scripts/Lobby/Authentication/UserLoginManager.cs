using System.Collections;
using UnityEngine;

namespace Game.Authentication
{
    using AeriaUtil.Validation;
    using Database.Payload;
    using Michsky.UI.Shift;
    using TMPro;
    using Mirror;

    public class UserLoginManager : MonoBehaviour, IAuthenticationManager
    {
        [Header("Main Menu Manager")]
        [SerializeField] private MainMenuManager _manager;

        [Header("Auth Fields")]
        [SerializeField] private TMP_InputField _loginName;
        [SerializeField] private TMP_InputField _password;

        [Header("Notification")]
        [SerializeField] private TextMeshProUGUI _loginUiNotificator;

        [Header("Login Animation")]
        [SerializeField] private Animator _splashScreenAnimator;
        [SerializeField] private string _animationName = "Login to Loading";

        [Header("Load-Out Animation")]
        [SerializeField] private TimedEvent _splashScreenTimedEvent;

        //Login Button Event
        public void ButtonLogin()
        {
            StartCoroutine(LoginAsync());
        }

        private IEnumerator LoginAsync()
        {
            // Validation
            bool loginTextIsValid = _loginName.text.
                NotEmpty(message => HandleOnError("Login", message)).
                MaxLength(50, message => HandleOnError("Login", message)).
                MinLength(3, message => HandleOnError("Login", message)).IsValid();

            if (!loginTextIsValid) yield break;

            bool passwordTextIsValid = _password.text.
                NotEmpty(message => HandleOnError("Password", message)).
                MaxLength(100, message => HandleOnError("Password", message)).
                MinLength(8, message => HandleOnError("Password", message)).IsValid();

            if (!passwordTextIsValid) yield break;

            //Payload
            UserLoginPayload request = new UserLoginPayload
            {
                loginName = _loginName.text,
                password = _password.text
            };

            //Login Request. Event Triggered on success
            yield return StartCoroutine(_manager.Login(request, HandleOnSuccess,HandleOnError));
        }

        private void HandleOnError(string field, string message)
        {
            _loginUiNotificator.text = $"{field}: {message}";
        }

        private void HandleOnError(string message)
        {
            _loginUiNotificator.text = message;
        }

        private void HandleOnSuccess()
        {
            _splashScreenAnimator.Play(_animationName);
            _splashScreenTimedEvent.StartIEnumerator();
        }
    }
}

