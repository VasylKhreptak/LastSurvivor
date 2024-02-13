using UnityEngine;
using Zenject;

namespace Gameplay.Entities.Loot.Variety.Core
{
    public abstract class Loot : MonoBehaviour
    {
        [Inject]
        private void Constructor(LootData lootData, Rigidbody rigidbody)
        {
            Data = lootData;
            Rigidbody = rigidbody;
        }

        public LootData Data { get; private set; }

        public Rigidbody Rigidbody { get; private set; }

        public abstract void RegisterLoot();
    }
}