using UI.Gameplay.Windows;
using UnityEngine;

namespace Zenject.Installers.SceneContext.Gameplay
{
    public class LevelUIInstaller : MonoInstaller
    {
        [Header("References")]
        [SerializeField] private StartWindow _startWindow;
        [SerializeField] private LevelCompletedWindow _levelCompletedWindow;

        #region MonoBehaviour

        private void OnValidate()
        {
            _startWindow ??= FindObjectOfType<StartWindow>(true);
            _levelCompletedWindow ??= FindObjectOfType<LevelCompletedWindow>(true);
        }

        #endregion

        public override void InstallBindings()
        {
            Container.BindInstance(_startWindow).AsSingle();
            Container.BindInstance(_levelCompletedWindow).AsSingle();
        }
    }
}