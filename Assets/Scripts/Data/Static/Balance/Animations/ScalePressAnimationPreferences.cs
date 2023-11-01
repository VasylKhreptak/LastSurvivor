using System;
using UnityEngine;

namespace Data.Static.Balance.Animations
{
    [Serializable]
    public class ScalePressAnimationPreferences
    {
        [Header("Preferences")]
        [SerializeField] private float _pressDuration;
        [SerializeField] private float _releaseDuration;
        [SerializeField] private Vector3 _pressedScale;
        [SerializeField] private Vector3 _releasedScale;
        [SerializeField] private AnimationCurve _pressCurve;
        [SerializeField] private AnimationCurve _releaseCurve;

        public float PressDuration => _pressDuration;
        public float ReleaseDuration => _releaseDuration;
        public Vector3 PressedScale => _pressedScale;
        public Vector3 ReleasedScale => _releasedScale;
        public AnimationCurve PressCurve => _pressCurve;
        public AnimationCurve ReleaseCurve => _releaseCurve;
    }
}