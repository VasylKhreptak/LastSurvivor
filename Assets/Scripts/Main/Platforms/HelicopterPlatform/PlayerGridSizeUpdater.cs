using System;
using Infrastructure.Services.PersistentData.Core;
using Main.Entities.Player;
using UniRx;
using Zenject;

namespace Main.Platforms.HelicopterPlatform
{
    public class PlayerGridSizeUpdater : IInitializable, IDisposable
    {
        private readonly IPersistentDataService _persistentDataService;
        private readonly Player _player;

        public PlayerGridSizeUpdater(IPersistentDataService persistentDataService, Player player)
        {
            _persistentDataService = persistentDataService;
            _player = player;
        }

        private IDisposable _subscription;

        public void Initialize() => StartObserving();

        public void Dispose() => StopObserving();

        private void StartObserving()
        {
            _subscription =
                _persistentDataService.Data.PlayerData.PlatformsData.HelicopterPlatformData.FuelTank.MaxValue
                    .Subscribe(OnFuelTankCapacityChanged);
        }

        private void StopObserving() => _subscription?.Dispose();

        private void OnFuelTankCapacityChanged(int capacity) => _player.BarrelGridStack.Bank.SetMaxValue(capacity);
    }
}