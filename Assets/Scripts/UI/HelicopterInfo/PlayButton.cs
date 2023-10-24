using Infrastructure.Services.PersistentData.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using UnityEngine.UI;
using Zenject.Infrastructure.Toggleable.Core;

namespace UI.HelicopterInfo
{
    public class PlayButton : IEnableable, IDisableable
    {
        private readonly Button _button;
        private readonly IStateMachine<IGameState> _stateMachine;
        private readonly IPersistentDataService _persistentDataService;

        public PlayButton(Button button, IStateMachine<IGameState> stateMachine, IPersistentDataService persistentDataService)
        {
            _button = button;
            _stateMachine = stateMachine;
            _persistentDataService = persistentDataService;
        }

        public void Enable()
        {
            _button.onClick.AddListener(LoadAppropriateLeve);
        }

        public void Disable()
        {
            _button.onClick.RemoveListener(LoadAppropriateLeve);
        }

        private void LoadAppropriateLeve() { }
    }
}