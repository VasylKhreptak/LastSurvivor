using System;
using UnityEngine;

namespace Inspector.MinMax.Core
{
    [Serializable]
    public abstract class MinMaxValue<T>
    {
        [SerializeField] private T _min;
        [SerializeField] private T _max;

        protected MinMaxValue(T min, T max)
        {
            _min = min;
            _max = max;
        }

        public T Min => _min;
        public T Max => _max;

        public abstract T GetRandom();
    }
}