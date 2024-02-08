using Inspector.MinMax.Core;
using UnityEngine;

namespace Inspector.MinMax
{
    public class IntMinMaxValue : MinMaxValue<int>
    {
        public IntMinMaxValue(int min, int max) : base(min, max) { }

        public override int GetRandom() => Random.Range(Min, Max);
    }
}