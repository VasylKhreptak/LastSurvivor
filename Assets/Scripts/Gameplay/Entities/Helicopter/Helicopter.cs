using UnityEngine;
using Utilities.SplineUtilities.TargetFollower.Core;
using Zenject;

namespace Gameplay.Entities.Helicopter
{
    public class Helicopter : MonoBehaviour
    {
        [Inject]
        private void Constructor(ISplineTargetFollower splineTargetFollower)
        {
            TargetFollower = splineTargetFollower;
        }

        public ISplineTargetFollower TargetFollower { get; private set; }
    }
}