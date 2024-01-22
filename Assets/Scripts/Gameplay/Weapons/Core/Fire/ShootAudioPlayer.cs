using Audio.Players;
using Plugins.AudioService.Core;
using Zenject.Infrastructure.Toggleable.Core;

namespace Gameplay.Weapons.Core.Fire
{
    public class ShootAudioPlayer : IEnableable, IDisableable
    {
        private readonly IAudioService _audioService;
        private readonly IWeapon _weapon;
        private readonly AudioPlayer.Preferences _preferences;

        private AudioPlayer _audioPlayer;

        public ShootAudioPlayer(IAudioService audioService, IWeapon weapon, AudioPlayer.Preferences preferences)
        {
            _audioService = audioService;
            _weapon = weapon;
            _preferences = preferences;

            _audioPlayer = new AudioPlayer(_audioService, _preferences);
        }

        public void Enable() => _weapon.OnShoot += Play;

        public void Disable() => _weapon.OnShoot -= Play;

        private void OnShoot(ShootData shootData) => Play(shootData);

        private void Play(ShootData shootData) => _audioPlayer.Play(shootData.Position);
    }
}