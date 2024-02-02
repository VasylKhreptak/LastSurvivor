using UnityEngine;
using Utilities.SplineUtilities;
using Zenject;

namespace Gameplay.Entities.Helicopter
{
    public class Helicopter : MonoBehaviour
    {
        [Inject]
        private void Constructor(SplineTargetFollower splineTargetFollower)
        {
            TargetFollower = splineTargetFollower;
        }

        public SplineTargetFollower TargetFollower { get; private set; }
    }
}