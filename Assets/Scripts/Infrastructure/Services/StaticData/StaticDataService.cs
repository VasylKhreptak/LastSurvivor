using System;
using Infrastructure.Data.Static;
using Infrastructure.Services.StaticData.Core;
using UnityEngine;

namespace Infrastructure.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string GameConfigPath = "StaticData/GameConfig";
        private const string GameBalancePath = "StaticData/GameBalance";
        
        private GameConfig _gameConfig;
        private GameBalance _gameBalance;

        public GameConfig Config => _gameConfig;
        public GameBalance Balance => _gameBalance;

        public void Load()
        {
            _gameConfig = Resources.Load<GameConfig>(GameConfigPath);
            _gameBalance = Resources.Load<GameBalance>(GameBalancePath);
        }
    }
}
