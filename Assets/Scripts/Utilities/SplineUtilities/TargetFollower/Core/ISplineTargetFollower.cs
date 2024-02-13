using UnityEngine;

namespace Utilities.SplineUtilities.TargetFollower.Core
{
    public interface ISplineTargetFollower
    {
        public Transform Target { get; set; }

        public void FollowTargetImmediately();
    }
}