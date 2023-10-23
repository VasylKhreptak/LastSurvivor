using System;
using Data.Persistent;
using TMPro;
using UniRx;

namespace UI.HelicopterInfo
{
    public class HelicopterFuelTankText
    {
        private IDisposable _subscription;

        private readonly TMP_Text _incomeMultiplierTMP;
        private readonly HelicopterData _helicopterData;

        public HelicopterFuelTankText(TMP_Text incomeMultiplierTMP, HelicopterData helicopterData)
        {
            _incomeMultiplierTMP = incomeMultiplierTMP;
            _helicopterData = helicopterData;
        }

        public void Enable() => StartObserving();

        public void Disable() => StopObserving();

        private void StartObserving()
        {
            StopObserving();

            _subscription = _helicopterData.IncomeMultiplier.Subscribe(OnValueChanged);
        }

        private void StopObserving()
        {
            _subscription?.Dispose();
        }

        private void OnValueChanged(float value)
        {
            _incomeMultiplierTMP.text = $"x{value:F2}";
        }
    }
}