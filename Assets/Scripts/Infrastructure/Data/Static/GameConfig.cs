using System.Collections.Generic;
using Data.Static;
using Udar.SceneManager;
using UnityEngine;
using LogType = Infrastructure.Services.Log.Core.LogType;

namespace Infrastructure.Data.Static
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/Static/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [Header("Scenes")]
        [SerializeField] private SceneField _bootstrapScene;
        [SerializeField] private SceneField _mainScene;
        [SerializeField] private List<SceneField> _levels;
        [SerializeField] private List<SceneField> _loopedLevels;

        [Header("Log Preferences")]
        [SerializeField] private LogType _editorLogType = LogType.Info;
        [SerializeField] private LogType _buildLogType = LogType.Info;

        [Header("Ads")]
        [SerializeField] private GoogleAdsSettings _googleAdsSettings;

        public SceneField BootstrapScene => _bootstrapScene;
        public SceneField MainScene => _mainScene;
        public IReadOnlyList<SceneField> Levels => _levels;
        public IReadOnlyList<SceneField> LoopedLevels => _loopedLevels;
        public LogType LogType => Application.isEditor ? _editorLogType : _buildLogType;

        public GoogleAdsSettings GoogleAdsSettings => _googleAdsSettings;
    }
}