using System;
using UnityEngine;

namespace Data.Static.Balance.Animations.Core
{
    [Serializable]
    public class AnimationPreferences
    {
        [Header("Preferences")]
        [SerializeField] private ScalePressAnimationPreferences _scalePressAnimationPreferences;

        public ScalePressAnimationPreferences ScalePressAnimationPreferences => _scalePressAnimationPreferences;
    }
}