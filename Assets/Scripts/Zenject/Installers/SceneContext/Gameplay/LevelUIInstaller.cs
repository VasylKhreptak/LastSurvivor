using UI.Gameplay.Windows;
using UnityEngine;

namespace Zenject.Installers.SceneContext.Gameplay
{
    public class LevelUIInstaller : MonoInstaller
    {
        [Header("References")]
        [SerializeField] private Canvas _canvas;
        [SerializeField] private StartWindow _startWindow;
        [SerializeField] private LevelCompletedWindow _levelCompletedWindow;
        [SerializeField] private WeaponAim _weaponAim;

        #region MonoBehaviour

        private void OnValidate()
        {
            _canvas ??= FindObjectOfType<Canvas>(true);
            _startWindow ??= FindObjectOfType<StartWindow>(true);
            _levelCompletedWindow ??= FindObjectOfType<LevelCompletedWindow>(true);
            _weaponAim ??= FindObjectOfType<WeaponAim>(true);
        }

        #endregion

        public override void InstallBindings()
        {
            Container.BindInstance(_canvas).AsSingle();
            Container.BindInstance(_startWindow).AsSingle();
            Container.BindInstance(_levelCompletedWindow).AsSingle();
            Container.BindInstance(_weaponAim).AsSingle();
        }
    }
}