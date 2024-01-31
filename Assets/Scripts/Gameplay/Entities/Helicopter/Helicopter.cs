using UnityEngine;
using Utilities.SplineUtilities;
using Zenject;

namespace Gameplay.Entities.Helicopter
{
    public class Helicopter : MonoBehaviour
    {
        private SplineTargetFollower _targetFollower;

        [Inject]
        private void Constructor(SplineTargetFollower splineTargetFollower)
        {
            _targetFollower = splineTargetFollower;
        }

        public SplineTargetFollower TargetFollower => _targetFollower;
    }
}