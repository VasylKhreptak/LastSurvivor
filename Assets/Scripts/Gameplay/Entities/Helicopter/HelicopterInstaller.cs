using Noise;
using UnityEngine;
using Zenject;

namespace Gameplay.Entities.Helicopter
{
    public class HelicopterInstaller : MonoInstaller
    {
        [Header("References")]
        [SerializeField] private Transform _transform;
        [SerializeField] private Transform _noiseLayer;

        [Header("Preferences")]
        [SerializeField] private HelicopterPlayerFollower.Preferences _helicopterMovementPreferences;
        [SerializeField] private NoiseRotator.Preferences _noiseRotatorPreferences;
        [SerializeField] private NoiseMover.Preferences _noiseMoverPreferences;

        #region MonoBehaviour

        private void OnValidate() => _transform ??= GetComponent<Transform>();

        #endregion

        public override void InstallBindings()
        {
            BindMovement();
        }

        private void BindMovement()
        {
            BindNoiseMovement();
            Container.BindInterfacesTo<HelicopterPlayerFollower>()
                .AsSingle()
                .WithArguments(_transform, _helicopterMovementPreferences);
        }

        private void BindNoiseMovement()
        {
            Container.BindInterfacesTo<NoiseRotator>().AsSingle().WithArguments(_noiseLayer, _noiseRotatorPreferences);
            Container.BindInterfacesTo<NoiseMover>().AsSingle().WithArguments(_noiseLayer, _noiseMoverPreferences);
        }
    }
}