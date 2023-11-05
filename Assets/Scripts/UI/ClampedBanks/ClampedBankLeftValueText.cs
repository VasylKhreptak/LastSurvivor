using System;
using Plugins.Banks;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace UI.ClampedBanks
{
    public class ClampedBankLeftValueText : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Text _tmp;

        private ClampedIntegerBank _bank;

        [Inject]
        private void Constructor(ClampedIntegerBank bank)
        {
            _bank = bank;
        }

        private IDisposable _subscription;

        #region MonoBehaviour

        private void OnValidate() => _tmp ??= GetComponent<TMP_Text>();

        private void OnEnable() => StartObserving();

        private void OnDisable() => StopObserving();

        #endregion

        private void StartObserving()
        {
            StopObserving();

            _subscription = _bank.LeftToFill.Subscribe(OnValueChanged);
        }

        private void StopObserving() => _subscription?.Dispose();

        private void OnValueChanged(int value) => _tmp.text = value.ToString();
    }
}