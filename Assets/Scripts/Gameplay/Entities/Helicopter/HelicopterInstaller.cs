using UnityEngine;
using UnityEngine.Splines;
using Utilities.Noise;
using Utilities.SplineUtilities.TargetFollower;
using Zenject;

namespace Gameplay.Entities.Helicopter
{
    public class HelicopterInstaller : MonoInstaller
    {
        [Header("References")]
        [SerializeField] private Transform _transform;
        [SerializeField] private Transform _noiseLayer;

        [Header("Preferences")]
        [SerializeField] private SplineContainer _splineContainer;
        [SerializeField] private SplineTargetFollower.Preferences _helicopterMovementPreferences;
        [SerializeField] private NoiseRotator.Preferences _noiseRotatorPreferences;
        [SerializeField] private NoiseMover.Preferences _noiseMoverPreferences;

        #region MonoBehaviour

        private void OnValidate() => _transform ??= GetComponent<Transform>();

        #endregion

        public override void InstallBindings()
        {
            Container.BindInstance(_transform).AsSingle();
            Container.BindInstance(_splineContainer);

            BindNoiseMovement();
            BindPlayerFollower();
        }

        private void BindPlayerFollower() =>
            Container.BindInterfacesAndSelfTo<FixedSplineTargetFollower>().AsSingle().WithArguments(_helicopterMovementPreferences);

        private void BindNoiseMovement()
        {
            Container.BindInterfacesTo<NoiseRotator>().AsSingle().WithArguments(_noiseLayer, _noiseRotatorPreferences);
            Container.BindInterfacesTo<NoiseMover>().AsSingle().WithArguments(_noiseLayer, _noiseMoverPreferences);
        }
    }
}