using System;
using Inspector.MinMax.Core;
using Random = UnityEngine.Random;

namespace Inspector.MinMax
{
    [Serializable]
    public class IntMinMaxValue : MinMaxValue<int>
    {
        public IntMinMaxValue(int min, int max) : base(min, max) { }

        public override int GetRandom() => Random.Range(Min, Max);
    }
}