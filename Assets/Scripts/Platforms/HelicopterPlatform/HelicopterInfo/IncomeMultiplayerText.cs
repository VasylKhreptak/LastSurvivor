using System;
using Infrastructure.Services.PersistentData.Core;
using TMPro;
using UniRx;
using Unity.VisualScripting;

namespace Platforms.HelicopterPlatform.HelicopterInfo
{
    public class IncomeMultiplayerText : IInitializable, IDisposable
    {
        private const string Format = "x{0}";

        private readonly TMP_Text _tmp;
        private readonly IReadOnlyReactiveProperty<float> _incomeMultiplayer;

        public IncomeMultiplayerText(TMP_Text tmp, IPersistentDataService persistentDataService)
        {
            _tmp = tmp;
            _incomeMultiplayer = persistentDataService.PersistentData.PlayerData.HelicopterPlatformData.HelicopterData
                .IncomeMultiplier;
        }

        private IDisposable _subscription;

        public void Initialize() => StartObserving();

        public void Dispose() => StopObserving();

        private void StartObserving()
        {
            StopObserving();

            _subscription = _incomeMultiplayer.Subscribe(OnValueChanged);
        }

        private void StopObserving() => _subscription?.Dispose();

        private void OnValueChanged(float value) => _tmp.text = string.Format(Format, value);
    }
}