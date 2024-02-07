using UnityEngine;

namespace Gameplay.Entities.Loot
{
    public class CollectHandler
    {
        private readonly Variety.Core.Loot _loot;
        private readonly GameObject _gameObject;

        public CollectHandler(Variety.Core.Loot loot, GameObject gameObject)
        {
            _loot = loot;
            _gameObject = gameObject;
        }

        public void HandleCollect()
        {
            _loot.RegisterLoot();
            _gameObject.transform.localScale = Vector3.one;
            _gameObject.SetActive(false);
        }
    }
}