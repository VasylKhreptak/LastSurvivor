using System;
using System.Collections.Generic;
using System.Linq;
using Plugins.AudioPoolSystem.Core;
using Plugins.AudioPoolSystem.Facade;
using Plugins.AudioPoolSystem.Facade.Core;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using AudioSettings = Plugins.AudioPoolSystem.Data.AudioSettings;
using Object = UnityEngine.Object;

namespace Plugins.AudioPoolSystem
{
    public class AudioPool : IAudioPool
    {
        private readonly Preferences _preferences;
        private readonly Transform _root;

        public AudioPool(Preferences preferences)
        {
            _preferences = preferences;

            GameObject rootGameObject = new GameObject("AudioPool");
            Object.DontDestroyOnLoad(rootGameObject);
            _root = rootGameObject.transform;

            Initialize();
        }

        private readonly HashSet<PooledObject> _totalPool = new HashSet<PooledObject>();
        private readonly HashSet<PooledObject> _activePool = new HashSet<PooledObject>();
        private readonly HashSet<PooledObject> _inactivePool = new HashSet<PooledObject>();

        private void Initialize() => Expand(_preferences.InitialSize);

        private void Expand()
        {
            GameObject gameObject = new GameObject("Audio");
            gameObject.transform.SetParent(_root);
            gameObject.SetActive(false);
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            Audio audio = new Audio(audioSource);

            PooledObject pooledObject = new PooledObject
            {
                Audio = audio,
                GameObject = gameObject
            };

            StartObserving(pooledObject);

            _totalPool.Add(pooledObject);
            _inactivePool.Add(pooledObject);
        }

        private void Expand(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Expand();
            }
        }

        public IAudio Get()
        {
            if (_inactivePool.Count > 0)
            {
                PooledObject pooledObject = _inactivePool.First();
                pooledObject.GameObject.SetActive(true);
                pooledObject.Audio.ApplySettings(_preferences.DefaultSettings);

                return pooledObject.Audio;
            }

            if (_totalPool.Count < _preferences.MaxSize)
            {
                Expand();
                return Get();
            }

            PooledObject lessImportantPooledObject = GetLessImportantPooledObject();
            lessImportantPooledObject.Audio.Clip = null;
            lessImportantPooledObject.Audio.ApplySettings(_preferences.DefaultSettings);

            return lessImportantPooledObject.Audio;
        }

        public IEnumerable<IAudio> GetAllActive() => _totalPool.Select(pooledObject => pooledObject.Audio);
        
        private PooledObject GetLessImportantPooledObject()
        {
            int priority = int.MinValue;

            PooledObject lessImportantPooledObject = null;

            foreach (PooledObject pooledObject in _activePool)
            {
                if (pooledObject.Audio.Priority > priority)
                {
                    priority = pooledObject.Audio.Priority;
                    lessImportantPooledObject = pooledObject;
                }
            }

            return lessImportantPooledObject;
        }

        private void StartObserving(PooledObject pooledObject)
        {
            pooledObject.Subscriptions.Add(pooledObject.GameObject.OnEnableAsObservable().Subscribe(_ => OnEnabled(pooledObject)));
            pooledObject.Subscriptions.Add(pooledObject.GameObject.OnDisableAsObservable().Subscribe(_ => OnDisabled(pooledObject)));
            pooledObject.Subscriptions.Add(pooledObject.GameObject.OnDestroyAsObservable().Subscribe(_ => OnDestroyed(pooledObject)));
        }

        private void StopObserving(PooledObject pooledObject) => pooledObject.Subscriptions.Clear();

        private void OnEnabled(PooledObject pooledObject)
        {
            _activePool.Add(pooledObject);
            _inactivePool.Remove(pooledObject);
        }

        private void OnDisabled(PooledObject pooledObject)
        {
            _activePool.Remove(pooledObject);
            _inactivePool.Add(pooledObject);
        }

        private void OnDestroyed(PooledObject pooledObject)
        {
            _totalPool.Remove(pooledObject);
            _activePool.Remove(pooledObject);
            _inactivePool.Remove(pooledObject);
            StopObserving(pooledObject);
        }

        [Serializable]
        public class Preferences
        {
            public int InitialSize = 10;
            public int MaxSize = 50;
            public AudioSettings DefaultSettings;
        }

        [Serializable]
        private class PooledObject
        {
            public IAudio Audio;
            public GameObject GameObject;
            public CompositeDisposable Subscriptions = new CompositeDisposable();
        }
    }
}