using UnityEngine;

namespace Gameplay.Weapons.Bullets.CollisionHandlers
{
    public class TrailReseter
    {
        private readonly TrailRenderer _renderer;

        public TrailReseter(TrailRenderer renderer)
        {
            _renderer = renderer;
        }

        public void Reset() => _renderer.Clear();
    }
}