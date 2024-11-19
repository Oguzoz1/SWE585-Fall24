using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Authentication
{
    using Database.Payload;
    using TMPro;
    public class RegistrationManager : MonoBehaviour, IAuthenticationManager
    {
        [Header("Main Menu Manager")]
        public MainMenuManager _manager;
        [Header("Auth Fields")]
        public TMP_InputField _registrationName;
        public TMP_InputField _registrationPassword;

        public void TryRegister()
        {
            StartCoroutine(RegisterAsync());
        }
        private IEnumerator RegisterAsync()
        {
            if (_registrationName == null || _registrationPassword == null)
            {
                //NOTIFY USER
                yield return null;
            }

            RegisterPayload request = new RegisterPayload
            {
                loginName = _registrationName.text,
                password = _registrationPassword.text
            };

            yield return _manager.Register(request);
        }
    }

}
