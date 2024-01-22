using UnityEngine;

namespace Gameplay.Weapons.Core
{
    public class ShootData
    {
        public readonly Vector3 Position;
        public readonly Vector3 Direction;
        
        public ShootData(Vector3 position, Vector3 direction)
        {
            Position = position;
            Direction = direction;
        }
    }
}