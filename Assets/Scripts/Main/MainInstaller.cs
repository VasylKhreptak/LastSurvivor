using Infrastructure.Services.Input.Main;
using Quests.Main.Core;
using UnityEngine;
using Zenject;

namespace Main
{
    public class MainInstaller : MonoInstaller
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
            BindInputService();
            BindQuestSequence();
        }

        private void BindInputService() => Container.BindInterfacesTo<MainInputService>().AsSingle().WithArguments(_joystick);

        private void BindQuestSequence() => Container.BindInterfacesTo<MainQuestSequence>().AsSingle();
    }
}