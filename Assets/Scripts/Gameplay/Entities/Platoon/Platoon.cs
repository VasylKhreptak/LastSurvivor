using System.Collections.Generic;
using UnityEngine;
using Utilities.SplineUtilities;
using Zenject;

namespace Gameplay.Entities.Platoon
{
    public class Platoon : MonoBehaviour
    {
        private List<Transform> _soldierPoints;
        public SplineTargetFollower TargetFollower { get; private set; }

        [Inject]
        private void Constructor(List<Transform> soldierPoints, SplineTargetFollower splineTargetFollower)
        {
            _soldierPoints = soldierPoints;
            TargetFollower = splineTargetFollower;
        }

        public IReadOnlyList<Transform> SoldierPoints => _soldierPoints;

        public readonly List<Soldier.Soldier> Soldiers = new List<Soldier.Soldier>();
    }
}