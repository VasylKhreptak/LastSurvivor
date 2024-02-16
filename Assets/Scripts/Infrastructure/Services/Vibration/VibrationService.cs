using Infrastructure.Services.Log.Core;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.Services.Vibration.Core;
using Lofelt.NiceVibrations;

namespace Infrastructure.Services.Vibration
{
    public class VibrationService : IVibrationService
    {
        private readonly IPersistentDataService _persistentDataService;
        private readonly ILogService _logService;

        public VibrationService(IPersistentDataService persistentDataService, ILogService logService)
        {
            _persistentDataService = persistentDataService;
            _logService = logService;
        }

        public void Vibrate(HapticPatterns.PresetType preset) => TryVibrate(preset);

        private void TryVibrate(HapticPatterns.PresetType preset)
        {
            if (_persistentDataService.Data.SettingsData.IsVibrationEnabled)
            {
                HapticPatterns.PlayPreset(preset);
                _logService.Log($"Vibrated with preset: {preset.ToString()}");
            }
        }
    }
}