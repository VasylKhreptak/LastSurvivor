using System;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Platforms.HelicopterPlatform.HelicopterInfo
{
    public class PlayButton : IInitializable, IDisposable
    {
        private readonly Button _button;
        private readonly IStateMachine<IGameState> _stateMachine;

        public PlayButton(Button button, IStateMachine<IGameState> stateMachine)
        {
            _button = button;
            _stateMachine = stateMachine;
        }

        private IDisposable _subscription;

        public void Initialize() => StartObserving();

        public void Dispose() => StopObserving();

        private void StartObserving()
        {
            StopObserving();

            _subscription = _button.OnClickAsObservable().Subscribe(_ => OnClicked());
        }

        private void StopObserving() => _subscription?.Dispose();

        private void OnClicked()
        {
            Debug.Log("OnClicked");
        }
    }
}