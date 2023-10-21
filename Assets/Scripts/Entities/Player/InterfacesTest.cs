using System;
using UnityEngine;
using Zenject;
using Zenject.Infrastructure.Toggleable.Core;

namespace Entities.Player
{
    public class InterfacesTest : IInitializable, IEnableable, IDisableable, IDisposable
    {
        public void Initialize()
        {
            Debug.Log("Initialized");
        }

        public void Enable()
        {
            Debug.Log("Enabled");
        }

        public void Disable()
        {
           Debug.Log("Disabled");
        }

        public void Dispose()
        {
            Debug.Log("Disposed");
        }
    }
}