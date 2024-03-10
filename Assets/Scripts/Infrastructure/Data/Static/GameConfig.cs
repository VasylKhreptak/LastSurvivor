using System.Collections.Generic;
using Audio;
using Data.Static;
using Notifications;
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

        [Header("Music")]
        [SerializeField] private BackgroundMusicPlayer.Preferences _backgroundMusicPreferences;

        [Header("Ads")]
        [SerializeField] private GoogleAdsSettings _googleAdsSettings;

        [Header("Retention Notifications")]
        [SerializeField] private List<BaseNotificationData> _retentionNotifications;

        public SceneField BootstrapScene => _bootstrapScene;
        public SceneField MainScene => _mainScene;
        public IReadOnlyList<SceneField> Levels => _levels;
        public IReadOnlyList<SceneField> LoopedLevels => _loopedLevels;
        public LogType LogType => Application.isEditor ? _editorLogType : _buildLogType;

        public BackgroundMusicPlayer.Preferences BackgroundMusicPreferences => _backgroundMusicPreferences;

        public GoogleAdsSettings GoogleAdsSettings => _googleAdsSettings;

        public IReadOnlyList<BaseNotificationData> RetentionNotifications => _retentionNotifications;
    }
}