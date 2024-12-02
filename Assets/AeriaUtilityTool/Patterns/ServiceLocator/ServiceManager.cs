using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AeriaUtil.Pattern.ServiceLocator
{
    public class ServiceManager
    {
        readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();
        public IEnumerable<object> RegisteredServices => _services.Values;
        /// <summary>
        /// If the designated service's type exists, the reference provided will get to the service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service"></param>
        /// <returns></returns>
        public bool TryGet<T>(out T service) where T : class
        {
            Type type = typeof(T);
            
            //If the value exists, make sure that service is that object as T value.
            if (_services.TryGetValue(type, out object serviceObj))
            {
                service = serviceObj as T;
                return true;
            }

            service = null;
            return false;
        }
        /// <summary>
        /// Get service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>False: Most likely It's not registered.</returns>
        public T Get<T>() where T : class
        {
            Type type = typeof(T);

            if (_services.TryGetValue(type, out object service))
                return service as T;

            throw new ArgumentException($"ServiceManager.Get: Service of type {type.FullName} is NOT registered");
        }
        /// <summary>
        /// Register a generic service with of type generic.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service"></param>
        /// <returns>Returns this ServiceManager object.</returns>
        public ServiceManager Register<T>(T service)
        {
            Type type = typeof(T);

            if (!_services.TryAdd(type, service))
                Debug.LogError($"ServiceManager.Register: Service of type {type.FullName} already registered");

            return this;
        }
        /// <summary>
        /// Register a specific service with a designated type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="service"></param>
        /// <returns>Returns this ServiceManager object.</returns>
        public ServiceManager Register(Type type, object service)
        {
            if (!type.IsInstanceOfType(service))
                throw new ArgumentException("Type of service does not match type of service interface", nameof(service));

            if (!_services.TryAdd(type, service))
                Debug.LogError($"ServiceManager.Register: Service of type {type.FullName} already registered");

            return this;
        }
    }
}

