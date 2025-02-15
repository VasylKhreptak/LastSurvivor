﻿using Gameplay.Data;
using Gameplay.Levels.Analytics;
using Gameplay.Levels.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using UI.Gameplay.Windows;

namespace Gameplay.Levels.StateMachine.States
{
    public class LevelCompletedState : ILevelState, IState
    {
        private readonly IStateMachine<ILevelState> _levelStateMachine;
        private readonly LevelData _levelData;
        private readonly LevelCompletedWindow _levelCompletedWindow;
        private readonly LevelEventLogger _levelEventLogger;

        public LevelCompletedState(IStateMachine<ILevelState> levelStateMachine, LevelData levelData,
            LevelCompletedWindow levelCompletedWindow, LevelEventLogger levelEventLogger)
        {
            _levelStateMachine = levelStateMachine;
            _levelData = levelData;
            _levelCompletedWindow = levelCompletedWindow;
            _levelEventLogger = levelEventLogger;
        }

        public void Enter()
        {
            _levelCompletedWindow.Show();
            _levelEventLogger.LogLevelCompletedEvent();
            _levelData.LevelResult = LevelResult.Completed;
            _levelStateMachine.Enter<PauseLevelState>();
        }
    }
}