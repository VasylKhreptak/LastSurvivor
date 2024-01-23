using Utilities.CameraUtilities.Shaker;
using Zenject.Infrastructure.Toggleable.Core;

namespace Gameplay.Weapons.Core.Fire
{
    public class ShootCameraShaker : IEnableable, IDisableable
    {
        private readonly IWeapon _weapon;
        private readonly CameraShaker _cameraShaker;

        public ShootCameraShaker(IWeapon weapon, CameraShaker cameraShaker)
        {
            _weapon = weapon;
            _cameraShaker = cameraShaker;
        }

        public void Enable() => _weapon.OnShoot += OnShoot;

        public void Disable() => _weapon.OnShoot -= OnShoot;

        private void OnShoot(ShootData shootData) => ShakeCamera();

        private void ShakeCamera() => _cameraShaker.DoShootShake();
    }
}