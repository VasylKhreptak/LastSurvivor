using Inspector.MinMax.Core;
using UnityEngine;

namespace Inspector.MinMax
{
    public class FloatMinMaxValue : MinMaxValue<float>
    {
        public FloatMinMaxValue(float min, float max) : base(min, max) { }

        public override float GetRandom() => Random.Range(Min, Max);
    }
}