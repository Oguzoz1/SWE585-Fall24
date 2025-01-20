using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Authentication
{
    using Database.Payload;
    using TMPro;
    using AeriaUtil.Validation;
    using System;

    public class RegistrationManager : MonoBehaviour, IAuthenticationManager
    {
        [Header("Main Menu Manager")]
        public MainMenuManager _manager;

        [Header("Auth Fields")]
        public TMP_InputField _registrationName;
        public TMP_InputField _registrationPassword;

        [Header("Notification")]
        [SerializeField] private TextMeshProUGUI _registerUinotificator;

        [Header("Register Animation")]
        [SerializeField] private Animator _splashScreenAnimator;
        [SerializeField] private string _animationName = "Sign Up to Login";


        public void TryRegister()
        {
            StartCoroutine(RegisterAsync());
        }
        private IEnumerator RegisterAsync()
        {
            bool registrationNameIsValid = _registrationName.text.
                NotEmpty(message => HandleOnError("Registration Name", message)).
                MinLength(3, message => HandleOnError("Registration Name", message)).
                MaxLength(50, message => HandleOnError("Registration Name", message)).
                Matches(@"^[a-zA-Z0-9_]+$", message => HandleOnError("Username can only contain, letters, numbers, and underscores", message)).
                IsValid();

            if (!registrationNameIsValid) yield break;

            bool passwordIsValid = _registrationPassword.text.
                NotEmpty(message => HandleOnError("Password", message)).
                MinLength(8, message => HandleOnError("Password", message)).
                MaxLength(100, message => HandleOnError("Password", message)).
                Matches(@"^[a-zA-Z0-9]+$", message => HandleOnError("Password must contain only letters and numbers.", message)).
                IsValid();
            if (!passwordIsValid) yield break;


            RegisterPayload request = new RegisterPayload
            {
                loginName = _registrationName.text,
                password = _registrationPassword.text
            };

            yield return _manager.Register(request, HandleOnSuccess,HandleOnError);
        }

        private void HandleOnSuccess()
        {
            _splashScreenAnimator.Play(_animationName);
        }

        private void HandleOnError(string prefixMessage, string message)
        {
            _registerUinotificator.text = $"{prefixMessage} {message}";
        }

        private void HandleOnError(string message)
        {
            _registerUinotificator.text = message;
        }
    }

}
