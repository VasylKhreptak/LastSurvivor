using System;
using Data.Persistent;
using Infrastructure.Services.PersistentData.Core;
using TMPro;
using UniRx;
using Unity.VisualScripting;

namespace Platforms.HelicopterPlatform.HelicopterInfo
{
    public class FuelTankText : IInitializable, IDisposable
    {
        private readonly TMP_Text _tmp;
        private readonly IReadOnlyReactiveProperty<int> _currentTankLevel;
        private readonly IReadOnlyReactiveProperty<int> _maxTankLevel;

        public FuelTankText(TMP_Text tmp, IPersistentDataService persistentDataService)
        {
            _tmp = tmp;
            HelicopterData helicopterData = persistentDataService.PersistentData.PlayerData.HelicopterPlatformData.HelicopterData;

            _currentTankLevel = helicopterData.FuelTank.Value;
            _maxTankLevel = helicopterData.FuelTank.MaxValue;
        }

        private readonly CompositeDisposable _subscriptions = new CompositeDisposable();

        public void Initialize() => StartObserving();

        public void Dispose() => StopObserving();

        private void StartObserving()
        {
            StopObserving();

            _subscriptions.Add(_currentTankLevel.Subscribe(_ => OnAnyValueChanged()));
            _subscriptions.Add(_maxTankLevel.Subscribe(_ => OnAnyValueChanged()));
        }

        private void StopObserving() => _subscriptions.Clear();

        private void OnAnyValueChanged()
        {
            _tmp.text = $"{_currentTankLevel.Value}/{_maxTankLevel.Value}";
        }
    }
}