using Infrastructure.Services.PersistentData.Core;
using Infrastructure.Services.Vibration.Core;
using Lofelt.NiceVibrations;

namespace Infrastructure.Services.Vibration
{
    public class VibrationService : IVibrationService
    {
        private readonly IPersistentDataService _persistentDataService;

        public VibrationService(IPersistentDataService persistentDataService) => _persistentDataService = persistentDataService;

        public void Vibrate(HapticPatterns.PresetType preset) => TryVibrate(preset);

        private void TryVibrate(HapticPatterns.PresetType preset)
        {
            if (_persistentDataService.PersistentData.SettingsData.IsVibrationEnabled)
                HapticPatterns.PlayPreset(preset);
        }
    }
}