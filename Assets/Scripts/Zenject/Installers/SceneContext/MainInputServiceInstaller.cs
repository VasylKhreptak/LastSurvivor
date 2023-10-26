using Infrastructure.Services.Input.Main;
using Infrastructure.Services.Input.Main.Core;
using UnityEngine;

namespace Zenject.Installers.SceneContext
{
    public class MainInputServiceInstaller : MonoInstaller
    {
        [Header("References")]
        [SerializeField] private Joystick _joystick;

        #region MonoBehaviour

        private void OnValidate()
        {
            _joystick ??= FindObjectOfType<Joystick>();
        }

        #endregion

        public override void InstallBindings()
        {
            IMainInputService inputService = new MainInputService(_joystick);
            Container.BindInstance(inputService).AsSingle();
        }
    }
}