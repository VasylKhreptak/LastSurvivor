using Data.Persistent;
using TMPro;
using UniRx;
using Zenject.Infrastructure.Toggleable.Core;

namespace UI.HelicopterInfo
{
    public class HelicopterIncomeMultiplierText : IEnableable, IDisableable
    {
        private readonly CompositeDisposable _subscriptions = new CompositeDisposable();

        private readonly TMP_Text _fuelTankTMP;
        private readonly HelicopterData _helicopterData;

        public HelicopterIncomeMultiplierText(TMP_Text fuelTankTMP, HelicopterData helicopterData)
        {
            _fuelTankTMP = fuelTankTMP;
            _helicopterData = helicopterData;
        }

        public void Enable() => StartObserving();

        public void Disable() => StopObserving();

        private void StartObserving()
        {
            StopObserving();

            _helicopterData.FuelTank.Value.Subscribe(_ => OnValuesChanged()).AddTo(_subscriptions);
            _helicopterData.FuelTank.MaxValue.Subscribe(_ => OnValuesChanged()).AddTo(_subscriptions);
        }

        private void StopObserving()
        {
            _subscriptions.Clear();
        }

        private void OnValuesChanged()
        {
            _fuelTankTMP.text = $"{_helicopterData.FuelTank.Value}/{_helicopterData.FuelTank.MaxValue}";
        }
    }
}