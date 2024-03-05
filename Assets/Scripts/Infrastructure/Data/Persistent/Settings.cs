namespace Infrastructure.Data.Persistent
{
    public class Settings
    {
        public float MusicVolume = 1f;
        public float SoundVolume = 1f;
        public bool IsVibrationEnabled = true;
        public int QualityLevel = 0;
        public int LocaleIndex = 0;
        public int SleepTimeout = UnityEngine.SleepTimeout.NeverSleep;
    }
}