using UI.Windows;
using UnityEngine;

namespace Zenject.Installers.SceneContext.Gameplay
{
    public class LevelUIInstaller : MonoInstaller
    {
        [Header("References")]
        [SerializeField] private StartWindow _startWindow;

        #region MonoBehaviour

        private void OnValidate()
        {
            _startWindow ??= FindObjectOfType<StartWindow>(true);
        }

        #endregion

        public override void InstallBindings()
        {
            Container.BindInstance(_startWindow).AsSingle();
        }
    }
}