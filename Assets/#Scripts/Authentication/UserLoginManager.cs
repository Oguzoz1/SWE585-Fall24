using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Authentication
{
    using Database.Payload;
    using Database.Services;
    using TMPro;
    public class UserLoginManager : MonoBehaviour, IAuthenticationManager
    {
        [Header("Main Menu Manager")]
        [SerializeField] private MainMenuManager _manager;
        [Header("Auth Fields")]
        [SerializeField] private TMP_InputField _loginName;
        [SerializeField] private TMP_InputField _password;

        public void TryLogin()
        {
            StartCoroutine(LoginAsync());
        }

        private IEnumerator LoginAsync()
        {
            if (_loginName == null || _password == null)
            {
                //Add a notification here
                Debug.LogError("Authentication fields are null");
                yield return null;
            }

            UserLoginPayload request = new UserLoginPayload
            {
                loginName = _loginName.text,
                password = _password.text
            };

            yield return StartCoroutine(_manager.Login(request));

        }
    }
}

