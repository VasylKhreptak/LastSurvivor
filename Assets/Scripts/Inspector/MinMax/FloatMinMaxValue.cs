using System;
using Inspector.MinMax.Core;
using Random = UnityEngine.Random;

namespace Inspector.MinMax
{
    [Serializable]
    public class FloatMinMaxValue : MinMaxValue<float>
    {
        public FloatMinMaxValue(float min, float max) : base(min, max) { }

        public override float GetRandom() => Random.Range(Min, Max);
    }
}