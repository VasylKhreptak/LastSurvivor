using System;
using Data.Persistent.Platforms;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace UI.Platforms.HelicopterPlatform
{
    public class IncomeMultiplayerText : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Text _tmp;

        [Header("Preferences")]
        [SerializeField] private string _format = "x{0}";

        private IReadOnlyReactiveProperty<float> _incomeMultiplier;

        [Inject]
        private void Constructor(HelicopterPlatformData platformData)
        {
            _incomeMultiplier = platformData.IncomeMultiplier;
        }

        private IDisposable _subscription;

        #region MonoBehaviour

        private void OnValidate() => _tmp ??= GetComponent<TMP_Text>();

        private void OnEnable() => StartObserving();

        private void OnDisable() => StopObserving();

        #endregion

        private void StartObserving() => _subscription = _incomeMultiplier.Subscribe(OnValueChanged);

        private void StopObserving() => _subscription?.Dispose();

        private void OnValueChanged(float value) => _tmp.text = string.Format(_format, value.ToString("F2").Replace(",", "."));
    }
}