using System;
using System.Collections.Generic;
using UnityEngine;

namespace Foundation.Services
{
    /// <summary>
    /// Central registry for game services.
    /// Register by interface, resolve from anywhere.
    /// </summary>
    public class ServiceLocator
    {

        private static readonly Dictionary<Type, object> _services = new();

        /// <summary>
        /// Register a service instance by its interface type.
        /// </summary>
        public static void Register<T>(T service) where T : class
        {
            Type type = typeof(T);

            if (_services.ContainsKey(type))
            {
                Debug.LogWarning($"[ServiceLocator] Overwriting {type.Name}");
            }

            _services[type] = service;
        }

        /// <summary>
        /// Get a registered service. Throws if not found.
        /// </summary>
        public static T Get<T>() where T : class
        {
            Type type = typeof(T);

            if (_services.TryGetValue(type, out object service))
            {
                return (T)service;
            }

            throw new InvalidOperationException(
                $"[ServiceLocator] {type.Name} not registered. " +
                $"Register it in Bootstrap before use.");
        }

        /// <summary>
        /// Try to get a service. Returns false if not found.
        /// </summary>
        public static bool TryGet<T>(out T service) where T : class
        {
            Type type = typeof(T);

            if (_services.TryGetValue(type, out object obj))
            {
                service = (T)obj;
                return true;
            }

            service = null;
            return false;
        }

        /// <summary>
        /// Remove a service from the registry.
        /// </summary>
        public static void Unregister<T>() where T : class
        {
            _services.Remove(typeof(T));
        }

        /// <summary>
        /// Clear all services. Call on application quit or scene reset.
        /// </summary>
        public static void Reset()
        {
            _services.Clear();
        }
    }
}
