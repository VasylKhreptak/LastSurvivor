using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utilities.GameObjectUtilities
{
    public class GameObjectRandomizer
    {
        private readonly Preferences _preferences;

        public GameObjectRandomizer(Preferences preferences)
        {
            _preferences = preferences;
        }

        public void Randomize()
        {
            int randomIndex = Random.Range(0, _preferences.GameObjects.Length);

            for (int i = 0; i < _preferences.GameObjects.Length; i++)
            {
                _preferences.GameObjects[i].SetActive(i == randomIndex);
            }
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private GameObject[] _gameObjects;

            public GameObject[] GameObjects => _gameObjects;
        }
    }
}