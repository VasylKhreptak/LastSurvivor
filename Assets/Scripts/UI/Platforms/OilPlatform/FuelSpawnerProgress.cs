using System;
using Main.Platforms.OilPlatform;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Platforms.OilPlatform
{
    public class FuelSpawnerProgress : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image _image;

        private IReadOnlyReactiveProperty<float> _progress;

        [Inject]
        private void Constructor(FuelSpawner fuelSpawner)
        {
            _progress = fuelSpawner.Timer.Progress;
        }

        private readonly int _propertyName = Shader.PropertyToID("_Arc1");

        private Material _material;

        private IDisposable _subscription;

        #region MoonBehaviour

        private void Awake() => _material = _image.materialForRendering;

        private void OnValidate() => _image ??= GetComponent<Image>();

        private void OnEnable() => StartObserving();

        private void OnDisable() => StopObserving();

        #endregion

        private void StartObserving() => _subscription = _progress.Subscribe(SetProgress);

        private void StopObserving() => _subscription?.Dispose();

        private void SetProgress(float progress) => _material.SetFloat(_propertyName, (1 - progress) * 360);
    }
}