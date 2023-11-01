using Data.Persistent;
using Infrastructure.Services.PersistentData.Core;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace UI.Platforms.HelicopterPlatform
{
    public class FuelTankText : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Text _tmp;

        [Header("Preferences")]
        [SerializeField] private string _format = "{0}/{1}";

        private IReadOnlyReactiveProperty<int> _currentTankLevel;
        private IReadOnlyReactiveProperty<int> _maxTankLevel;

        [Inject]
        private void Constructor(IPersistentDataService persistentDataService)
        {
            HelicopterData helicopterData = persistentDataService.PersistentData.PlayerData.HelicopterPlatformData.HelicopterData;

            _currentTankLevel = helicopterData.FuelTank.Value;
            _maxTankLevel = helicopterData.FuelTank.MaxValue;
        }

        private readonly CompositeDisposable _subscriptions = new CompositeDisposable();

        #region MonoBehaivour

        private void OnValidate() => _tmp ??= GetComponent<TMP_Text>();

        private void OnEnable() => StartObserving();

        private void OnDisable() => StopObserving();

        #endregion

        private void StartObserving()
        {
            _currentTankLevel.Subscribe(_ => OnAnyValueChanged()).AddTo(_subscriptions);
            _maxTankLevel.Subscribe(_ => OnAnyValueChanged()).AddTo(_subscriptions);
        }

        private void StopObserving() => _subscriptions.Clear();

        private void OnAnyValueChanged() => _tmp.text = string.Format(_format, _currentTankLevel.Value, _maxTankLevel.Value);
    }
}