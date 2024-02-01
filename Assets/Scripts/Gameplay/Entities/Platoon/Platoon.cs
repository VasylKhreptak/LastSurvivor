using System.Collections.Generic;
using UnityEngine;
using Utilities.SplineUtilities;
using Zenject;

namespace Gameplay.Entities.Platoon
{
    public class Platoon : MonoBehaviour
    {
        private List<Transform> _soldierPoints;
        private SplineTargetFollower _targetFollower;

        [Inject]
        private void Constructor(List<Transform> soldierPoints, SplineTargetFollower splineTargetFollower)
        {
            _soldierPoints = soldierPoints;
            _targetFollower = splineTargetFollower;
        }

        public IReadOnlyList<Transform> SoldierPoints => _soldierPoints;
        public SplineTargetFollower TargetFollower => _targetFollower;
    }
}