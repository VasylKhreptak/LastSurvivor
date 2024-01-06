using UnityEngine;
using Zenject.Infrastructure.Toggleable.Core;

namespace Gameplay.Weapons.Bullets.CollisionHandlers
{
    public class TrailReseter : IDisableable
    {
        private readonly TrailRenderer _renderer;

        public TrailReseter(TrailRenderer renderer)
        {
            _renderer = renderer;
        }

        public void Disable() => Reset();

        private void Reset() => _renderer.Clear();
    }
}