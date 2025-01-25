using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Authentication
{
    public enum AuthenticateeRoles
    {
        Player,
        Moderator,
        Admin
    }
    public static class AuthenticatoryFactory
    {
        private static readonly Dictionary<AuthenticateeRoles, Func<IAuthenticatorService>> _authenticatorRegistery = new();
        /// <summary>
        /// Initialize the factory. Add new authenticator services on static constructor.
        /// </summary>
        static AuthenticatoryFactory()
        {
            Register(AuthenticateeRoles.Player, () => new PlayerAuthenticator());
        }
        /// <summary>
        /// Register a new authenticator service with a designated role.
        /// </summary>
        /// <param name="role"></param>
        /// <param name="authenticatorCreator"></param>
        private static void Register(AuthenticateeRoles role, Func<IAuthenticatorService> authenticatorCreator)
        {
            if (_authenticatorRegistery.ContainsKey(role))
                throw new ArgumentException($"Authenticator for user type {role} is already registered;");

            _authenticatorRegistery.Add(role, authenticatorCreator);
        }
        /// <summary>
        /// Get the authenticator service from the dictionary.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public static IAuthenticatorService CreateAuthenticator(AuthenticateeRoles role)
        {
            if (_authenticatorRegistery.TryGetValue(role, out var authenticatorCreator))
                return authenticatorCreator();
            throw new ArgumentException($"Unsupported user type: {role}");
        }
    }
}
