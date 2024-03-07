namespace Analytics
{
    public static class AnalyticEvents
    {
        public const string ApplicationOpen = "custom_application_open";
        public const string ApplicationClose = "custom_application_close";
        public const string FailedFirstLevel = "custom_failed_first_level";
        public const string LevelLoaded = "custom_level_loaded";
        public const string LevelStarted = "custom_level_started";
        public const string LevelCompleted = "custom_level_completed";
        public const string LevelFailed = "custom_level_failed";
        public const string LevelEnded = "custom_level_ended";
        public const string PlayerDied = "custom_player_died";
        public const string PlayerRevived = "custom_player_revived";
        public const string ZombieDied = "custom_zombie_died";
        public const string SceneLoaded = "custom_scene_loaded";
        public const string SceneUnloaded = "custom_scene_unloaded";
        public const string ClickedButton = "custom_clicked_button";
        public const string BoughtPlatform = "custom_bought_platform";
        public const string HiredEntity = "custom_hired_entity";
        public const string UpgradedPlatform = "custom_upgraded_platform";
        public const string SavedData = "custom_saved_data";
        public const string CurrencyChanged = "custom_currency_changed";
        public const string SetVolume = "custom_set_volume";
        public const string SetLanguage = "custom_set_language";
        public const string SetVibration = "custom_set_vibration";
        public const string SetQuality = "custom_set_quality";
        public const string ApplicationPaused = "custom_application_paused";
        public const string ApplicationResumed = "custom_application_resumed";
        public const string CompletedQuest = "custom_completed_quest";
        public const string BarrelExploded = "custom_barrel_exploded";
        public const string BrokeLootBox = "custom_broke_lootbox";
        public const string AdReward = "custom_ad_reward";
        public const string StartedIdling = "custom_started_idling";
        public const string StoppedIdling = "custom_stopped_idling";
    }
}