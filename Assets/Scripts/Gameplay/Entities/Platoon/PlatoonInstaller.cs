using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Utilities.SplineUtilities;
using Zenject;

namespace Gameplay.Entities.Platoon
{
    public class PlatoonInstaller : MonoInstaller
    {
        [SerializeField] private SplineContainer _splineContainer;
        [SerializeField] private List<Transform> _soldierPoints;
        [SerializeField] private SplineTargetFollower.Preferences _movePreferences;

        public override void InstallBindings()
        {
            Container.BindInstance(transform).AsSingle();
            Container.BindInstance(_splineContainer).AsSingle();
            Container.BindInstance(_soldierPoints).AsSingle();

            BindMovement();
        }

        private void BindMovement() =>
            Container.BindInterfacesAndSelfTo<SplineTargetFollower>().AsSingle().WithArguments(_movePreferences);
    }
}