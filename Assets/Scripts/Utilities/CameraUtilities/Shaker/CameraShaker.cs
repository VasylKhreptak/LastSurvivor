using System;
using Extensions;
using Noise;
using UnityEngine;
using Zenject;

namespace CameraUtilities.Shaker
{
    public class CameraShaker : ITickable, IDisposable
    {
        private readonly Camera _camera;
        private readonly Preferences _preferences;

        private readonly NoiseRotator _baseNoiseRotator;
        private readonly ShakeLayer _fireLayer;
        private readonly ShakeLayer _explodeLayer;

        public CameraShaker(Camera camera, Preferences preferences)
        {
            _camera = camera;
            _preferences = preferences;

            Transform fireLayerTransform = _camera.transform.CreateParent("Fire Layer");
            Transform explodeLayerTransform = fireLayerTransform.CreateParent("Explode Layer");

            _fireLayer = new ShakeLayer(fireLayerTransform, _preferences.FireLayerPreferences);
            _explodeLayer = new ShakeLayer(explodeLayerTransform, _preferences.ExplodeLayerPreferences);
            _baseNoiseRotator = new NoiseRotator(_camera.transform, _preferences.BaseNoisePreferences);
        }

        public void Tick()
        {
            _baseNoiseRotator.Tick();
            _fireLayer.Tick();
            _explodeLayer.Tick();
        }

        public void Dispose()
        {
            _fireLayer.Dispose();
            _explodeLayer.Dispose();
        }

        public void DoShootShake() => _fireLayer.Shake();

        public void DoExplosionShake() => _explodeLayer.Shake();

        [Serializable]
        public class Preferences
        {
            [SerializeField] private NoiseRotator.Preferences _baseNoisePreferences;
            [SerializeField] private ShakeLayer.Preferences _fireLayerPreferences;
            [SerializeField] private ShakeLayer.Preferences _explodeLayerPreferences;

            public NoiseRotator.Preferences BaseNoisePreferences => _baseNoisePreferences;
            public ShakeLayer.Preferences FireLayerPreferences => _fireLayerPreferences;
            public ShakeLayer.Preferences ExplodeLayerPreferences => _explodeLayerPreferences;
        }
    }
}