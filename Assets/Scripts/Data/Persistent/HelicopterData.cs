using Plugins.Banks;
using UnityEngine;

namespace Data.Persistent
{
    public class HelicopterData
    {
        public float IncomeMultiplier = 1f;
        public ClampedIntegerBank FuelTank = new ClampedIntegerBank(0, 5);

        public bool CanTakeOff => Mathf.Approximately(FuelTank.FillAmount.Value, 1f);
    }
}