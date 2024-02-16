using Gameplay.Entities.Player;
using Gameplay.Levels.StateMachine.States;
using Gameplay.Levels.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Gameplay.Levels
{
    public class LevelRunner : MonoBehaviour, IPointerDownHandler
    {
        private IStateMachine<ILevelState> _levelStateMachine;

        [Inject]
        private void Constructor(IStateMachine<ILevelState> levelStateMachine, PlayerHolder playerHolder)
        {
            _levelStateMachine = levelStateMachine;
        }

        public void OnPointerDown(PointerEventData eventData) => StartLevel();

        private void StartLevel()
        {
            _levelStateMachine.Enter<LevelStartState>();
            Destroy(this);
        }
    }
}