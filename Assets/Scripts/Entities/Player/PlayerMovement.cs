using Infrastructure.Data.Static.Balance;
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

        public void Tick() => HandleMovement();

        private void HandleMovement()
        {
            Vector2 input = _mainInputService.Direction;

            if (Mathf.Approximately(input.magnitude, 0f))
                return;

            Vector3 moveDirection = new Vector3(input.x, 0f, input.y);
            Vector3 verticalVelocity = Vector3.up * _rigidbody.velocity.y;

            _transform.forward = moveDirection;
            _rigidbody.velocity = moveDirection * _playerPreferences.MovementSpeed + verticalVelocity;
        }
    }
}