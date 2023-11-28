using System;
using Data.Static.Balance.Platforms;
using Plugins.Banks;
using UniRx;
using Zenject;

namespace Main.Platforms.RecruitmentLogic
{
    public class EntityPriceUpdater : IInitializable, IDisposable
    {
        private readonly ClampedIntegerBank _entityBank;
        private readonly ClampedIntegerBank _entityHireBank;
        private readonly EntityHirePricePreferences _pricePreferences;

        public EntityPriceUpdater(ClampedIntegerBank entityBank, ClampedIntegerBank entityHireBank,
            EntityHirePricePreferences pricePreferences)
        {
            _entityBank = entityBank;
            _entityHireBank = entityHireBank;
            _pricePreferences = pricePreferences;
        }

        private IDisposable _subscription;

        public void Initialize() => StartObserving();

        public void Dispose() => StopObserving();

        private void StartObserving() => _subscription = _entityBank.Value.Subscribe(OnEntityCountChanged);

        private void StopObserving() => _subscription?.Dispose();

        private void OnEntityCountChanged(int count)
        {
            _entityHireBank.SetMaxValue(_pricePreferences.BasePrice + _pricePreferences.PriceIncrement * count);
        }
    }
}