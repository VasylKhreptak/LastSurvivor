using Lofelt.NiceVibrations;

namespace Infrastructure.Services.Vibration.Core
{
    public interface IVibrationService
    {
        public void Vibrate(HapticPatterns.PresetType preset);
    }
}