using Gameplay.Entities.Player.StateMachine.States;
using Gameplay.Entities.Player.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;
using UnityEngine;
using Zenject;

namespace Gameplay.Entities.Player
{
    public class PlayerController : ITickable
    {
        private readonly Camera _camera;
        private IStateMachine<IPlayerState> _playerStateMachine;

        public PlayerController(Camera camera, IStateMachine<IPlayerState> playerStateMachine)
        {
            _camera = camera;
            _playerStateMachine = playerStateMachine;
        }

        public void Tick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    MoveState.Payload payload = new MoveState.Payload
                    {
                        Position = hit.point,
                        OnComplete = () => Debug.Log("Move Completed")
                    };

                    _playerStateMachine.Enter<MoveState, MoveState.Payload>(payload);
                }
            }
        }
    }
}