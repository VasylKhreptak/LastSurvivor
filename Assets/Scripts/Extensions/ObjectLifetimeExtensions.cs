using System;
using Lifetime;
using UnityEngine;
using Zenject;
using Zenject.Infrastructure.Toggleable.Core;

namespace Extensions
{
    public static class ObjectLifetimeExtensions
    {
        public static void ShareLifetimeWith(this GameObject gameObject, IInitializable initializable) =>
            gameObject.TryAddComponent<ObjectLifetimeProvider>().Add(initializable);

        public static void ShareLifetimeWith(this GameObject gameObject, IEnableable enableable) =>
            gameObject.TryAddComponent<ObjectLifetimeProvider>().Add(enableable);

        public static void ShareLifetimeWith(this GameObject gameObject, ITickable tickable) =>
            gameObject.TryAddComponent<ObjectLifetimeProvider>().Add(tickable);

        public static void ShareLifetimeWith(this GameObject gameObject, IFixedTickable fixedTickable) =>
            gameObject.TryAddComponent<ObjectLifetimeProvider>().Add(fixedTickable);

        public static void ShareLifetimeWith(this GameObject gameObject, ILateTickable lateTickable) =>
            gameObject.TryAddComponent<ObjectLifetimeProvider>().Add(lateTickable);

        public static void ShareLifetimeWith(this GameObject gameObject, IDisableable disableable) =>
            gameObject.TryAddComponent<ObjectLifetimeProvider>().Add(disableable);

        public static void ShareLifetimeWith(this GameObject gameObject, IDisposable disposable) =>
            gameObject.TryAddComponent<ObjectLifetimeProvider>().Add(disposable);
    }
}