using UnityEngine;
using Zenject;

namespace Gameplay.Entities.Loot.Variety.Core
{
    public abstract class Loot : MonoBehaviour
    {
        private LootData _data;
        private Rigidbody _rigidbody;

        [Inject]
        private void Constructor(LootData lootData, Rigidbody rigidbody)
        {
            _data = lootData;
            _rigidbody = rigidbody;
        }

        public LootData Data => _data;
        public Rigidbody Rigidbody => _rigidbody;

        public abstract void RegisterLoot();
    }
}