using System;
using Infrastructure.Services.PersistentData.Core;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace UI.Resources
{
    public class MoneyText : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Text _tmp;

        private Data.Persistent.Resources _resources;

        [Inject]
        private void Constructor(IPersistentDataService persistentDataService)
        {
            _resources = persistentDataService.PersistentData.PlayerData.Resources;
        }

        private IDisposable _subscription;

        #region MonoBehaviour

        private void OnValidate() => _tmp ??= GetComponent<TMP_Text>();

        private void OnEnable() => StartObserving();

        private void OnDisable() => StopObserving();

        #endregion

        private void StartObserving() => _subscription = _resources.Money.Value.Subscribe(OnValueChanged);

        private void StopObserving() => _subscription?.Dispose();

        private void OnValueChanged(int value) => _tmp.text = value.ToString();
    }
}