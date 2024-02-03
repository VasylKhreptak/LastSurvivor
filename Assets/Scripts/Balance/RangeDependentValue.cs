using System;
using Extensions;
using UnityEngine;

namespace Balance
{
    [Serializable]
    public class RangeDependentValue
    {
        [SerializeField] private float _minIn;
        [SerializeField] private float _maxIn;
        [SerializeField] private float _minOut;
        [SerializeField] private float _maxOut;
        [SerializeField] private AnimationCurve _curve;

        public float Get(float value) => _curve.Evaluate(_minIn, _maxIn, value, _minOut, _maxOut);
    }
}