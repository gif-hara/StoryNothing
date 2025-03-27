using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace HK
{
    public class ServiceLocator
    {
        private static readonly Dictionary<Type, object> services = new();

        private static readonly Dictionary<Type, Dictionary<string, object>> namedServices = new();

        private static async UniTaskVoid RegisterAsync<T>(T service, CancellationToken cancellationToken = default)
        {
            if (services.ContainsKey(typeof(T)))
            {
                Debug.LogError($"Service already registered: {typeof(T)}");
                return;
            }
            var scope = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
#if UNITY_EDITOR
            scope = CancellationTokenSource.CreateLinkedTokenSource(Application.exitCancellationToken);
#endif
            services[typeof(T)] = service;
            await UniTask.WaitUntilCanceled(scope.Token, completeImmediately: true);
            services.Remove(typeof(T));
        }

        private static async UniTaskVoid RegisterAsync<T>(string name, T service, CancellationToken cancellationToken = default)
        {
            if (!namedServices.TryGetValue(typeof(T), out var namedService))
            {
                namedService = new Dictionary<string, object>();
                namedServices.Add(typeof(T), namedService);
            }

            if (namedService.ContainsKey(name))
            {
                Debug.LogError($"Service already registered: {typeof(T)}, name: {name}");
                return;
            }

            var scope = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
#if UNITY_EDITOR
            scope = CancellationTokenSource.CreateLinkedTokenSource(Application.exitCancellationToken);
#endif
            namedService[name] = service;
            await UniTask.WaitUntilCanceled(scope.Token, completeImmediately: true);
            namedService.Remove(name);
        }

        public static void Register<T>(T service, CancellationToken cancellationToken)
        {
            RegisterAsync(service, cancellationToken).Forget();
        }

        public static void Register<T>(string name, T service, CancellationToken cancellationToken)
        {
            RegisterAsync(name, service, cancellationToken).Forget();
        }

        public static T Resolve<T>()
        {
            Assert.IsTrue(services.ContainsKey(typeof(T)), $"Service not found: {typeof(T)}");
            return (T)services[typeof(T)];
        }

        public static T Resolve<T>(string name)
        {
            Assert.IsTrue(namedServices.ContainsKey(typeof(T)), $"Service not found: {typeof(T)}");
            Assert.IsTrue(namedServices[typeof(T)].ContainsKey(name), $"Service not found: {typeof(T)}, name: {name}");
            return (T)namedServices[typeof(T)][name];
        }

        public static T TryResolve<T>()
        {
            return services.ContainsKey(typeof(T)) ? (T)services[typeof(T)] : default;
        }

        public static T TryResolve<T>(string name)
        {
            return namedServices.ContainsKey(typeof(T)) && namedServices[typeof(T)].ContainsKey(name) ? (T)namedServices[typeof(T)][name] : default;
        }

        public static bool Contains<T>()
        {
            return services.ContainsKey(typeof(T));
        }

        public static bool Contains<T>(string name)
        {
            return namedServices.ContainsKey(typeof(T)) && namedServices[typeof(T)].ContainsKey(name);
        }
    }
}