using Data.Static.Balance;
using Infrastructure.Services.Input.Main.Core;
using Infrastructure.Services.StaticData.Core;
using UnityEngine;
using Zenject;

namespace Entities.Player
{
    public class PlayerMovement : ITickable
    {
        private readonly Transform _transform;
        private readonly Rigidbody _rigidbody;
        private readonly IMainInputService _mainInputService;
        private readonly PlayerPreferences _playerPreferences;

        public PlayerMovement(Transform transform, Rigidbody rigidbody, IMainInputService mainInputService,
            IStaticDataService staticDataService)
        {
            _transform = transform;
            _rigidbody = rigidbody;
            _mainInputService = mainInputService;
            _playerPreferences = staticDataService.Balance.PlayerPreferences;
        }

        public void Tick() => HandleInput();

        private void HandleInput()
        {
            Vector2 input = _mainInputService.Direction;

            if (Mathf.Approximately(input.magnitude, 0f))
                return;

            Vector3 targetForward = new Vector3(input.x, 0f, input.y);
            Vector3 currentMoveDirection =
                Vector3.Lerp(_transform.forward, targetForward, _playerPreferences.RotateSpeed * Time.deltaTime);

            _rigidbody.MoveRotation(Quaternion.LookRotation(currentMoveDirection));
            _rigidbody.velocity = currentMoveDirection * _playerPreferences.Velocity;
        }
    }
}