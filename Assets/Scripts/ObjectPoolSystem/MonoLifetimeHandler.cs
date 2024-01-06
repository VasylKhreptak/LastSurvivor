using System;
using UniRx;
using UnityEngine;

namespace ObjectPoolSystem
{
    public class MonoLifetimeHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _gameObject;

        [Header("Preferences")]
        [SerializeField] private float _lifetime = 1f;

        private IDisposable _subscription;

        #region MonoBehaviour

        private void OnValidate() => _gameObject ??= GetComponent<GameObject>();

        private void OnEnable() => StartTimer();

        private void OnDisable() => StopTimer();

        #endregion

        private void StartTimer()
        {
            _subscription = Observable
                .Timer(TimeSpan.FromSeconds(_lifetime))
                .Subscribe(_ => _gameObject.SetActive(false));
        }

        private void StopTimer() => _subscription?.Dispose();
    }
}