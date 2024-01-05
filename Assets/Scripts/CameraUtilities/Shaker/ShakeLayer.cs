using System;
using UnityEngine;
using Zenject;

namespace CameraUtilities.Shaker
{
    public class ShakeLayer : ITickable, IDisposable
    {
        
        public void Tick() { }

        public void Dispose() { }

        [Serializable]
        public class Preferences
        {
            
        }
    }
}