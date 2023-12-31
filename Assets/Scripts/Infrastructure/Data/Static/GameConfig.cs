using UnityEngine;
using LogType = Infrastructure.Services.Log.Core.LogType;

namespace Infrastructure.Data.Static
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/Static/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [Header("Scenes")]
        [SerializeField] private string _bootstrapScene = "Bootstrap";
        [SerializeField] private string _mainScene = "MainScene";
        [SerializeField] private string _tutorialScene = "TutorialScene";
        [SerializeField] private string _level1Scene = "Level1";

        [Header("Log Preferences")]
        [SerializeField] private LogType _editorLogType = LogType.Info;
        [SerializeField] private LogType _buildLogType = LogType.Info;

        public string BootstrapScene => _bootstrapScene;
        public string MainScene => _mainScene;
        public string TutorialScene => _tutorialScene;
        public string Level1Scene => _level1Scene;

        public LogType LogType => Application.isEditor ? _editorLogType : _buildLogType;
    }
}