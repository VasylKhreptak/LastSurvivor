using System;
using Main.Platforms.BuyZones.Core;
using UnityEngine;
using Zenject;
using IInitializable = Unity.VisualScripting.IInitializable;

namespace Main.Platforms
{
    public class PlatformBinder<T> : IInitializable, IDisposable
    {
        private readonly DiContainer _container;
        private readonly PlatformBuyer _platformBuyer;

        public PlatformBinder(DiContainer container, PlatformBuyer platformBuyer)
        {
            _container = container;
            _platformBuyer = platformBuyer;
        }

        public void Initialize() => _platformBuyer.OnBought += OnBoundPlatform;

        public void Dispose() => _platformBuyer.OnBought -= OnBoundPlatform;

        private void OnBoundPlatform(GameObject platform) => _container.Bind<T>().FromComponentOn(platform).AsSingle();
    }
}