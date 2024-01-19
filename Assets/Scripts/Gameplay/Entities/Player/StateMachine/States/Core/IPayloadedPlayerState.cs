using Infrastructure.StateMachine.Main.States.Core;

namespace Gameplay.Entities.Player.StateMachine.States.Core
{
    public interface IPayloadedPlayerState<in TPayload> : IPayloadedState<TPayload> { }
}