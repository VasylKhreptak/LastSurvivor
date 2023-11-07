using System;
using Data.Persistent;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace UI.Platforms.OilPlatform
{
    public class BarrelsPerSecondText : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Text _tmp;

        [Header("Preferences")]
        [SerializeField] private string _format = "x{0}/s";
        [SerializeField] private string _valueFormat = "0.0";

        private OilPlatformData _platformData;

        [Inject]
        private void Constructor(OilPlatformData platformData)
        {
            _platformData = platformData;
        }

        private IDisposable _subscription;

        #region MonoBehaviour

        private void OnValidate() => _tmp ??= GetComponent<TMP_Text>();

        private void OnEnable() => StartObserving();

        private void OnDisable() => StopObserving();

        #endregion

        private void StartObserving() => _subscription = _platformData.BarrelProduceDuration.Subscribe(OnValueChanged);

        private void StopObserving() => _subscription?.Dispose();

        private void OnValueChanged(float barrelProduceDuration) =>
            _tmp.text = string.Format(_format, barrelProduceDuration.ToString(_valueFormat).Replace(',', '.'));
    }
}