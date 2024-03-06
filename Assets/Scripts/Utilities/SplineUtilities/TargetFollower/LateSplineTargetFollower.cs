using UnityEngine;
using UnityEngine.Splines;
using Utilities.SplineUtilities.TargetFollower.Core;
using Zenject;

namespace Utilities.SplineUtilities.TargetFollower
{
    public class LateSplineTargetFollower : ISplineTargetFollower, ILateTickable
    {
        private readonly SplineTargetFollower _follower;

        public LateSplineTargetFollower(Transform transform, SplineContainer splineContainer,
            SplineTargetFollower.Preferences preferences)
        {
            _follower = new SplineTargetFollower(transform, splineContainer, preferences);
        }

        public void LateTick() => _follower.Tick();

        public Transform Target
        {
            get => _follower.Target;
            set => _follower.Target = value;
        }

        public void FollowTargetImmediately() => _follower.FollowTargetImmediately();
    }
}