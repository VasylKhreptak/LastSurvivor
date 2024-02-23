using System;
using Gameplay.Weapons.Core;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Gameplay.ProgressBars
{
    public class WeaponReloadProgressBar : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image _image;

        private IWeapon _weapon;

        [Inject]
        private void Constructor(IWeapon weapon)
        {
            _weapon = weapon;
        }

        private readonly int _propertyName = Shader.PropertyToID("_Arc2");

        private Material _material;

        private IDisposable _progressSubscription;

        #region MoonBehaviour

        private void Awake() => _material = _image.materialForRendering;

        private void OnValidate() => _image ??= GetComponent<Image>();

        private void OnEnable() => StartObservingProgress();

        private void OnDisable() => StopObservingProgress();

        #endregion

        private void StartObservingProgress() => _progressSubscription = _weapon.ReloadProgress.Subscribe(SetProgress);

        private void StopObservingProgress() => _progressSubscription?.Dispose();

        private void SetProgress(float progress)
        {
            if (Mathf.Approximately(progress, 0f))
                return;

            _material.SetFloat(_propertyName, (1 - progress) * 360);
        }
    }
}