using Data.Static.Balance;
using Infrastructure.Services.StaticData.Core;
using UnityEngine;
using Zenject;

namespace Entities.Player
{
    public class PlayerMoveAnimation : ITickable
    {
        private readonly Animator _animator;
        private readonly Rigidbody _rigidbody;
        private readonly PlayerPreferences _playerPreferences;

        public PlayerMoveAnimation(Animator animator, Rigidbody rigidbody, IStaticDataService staticDataService)
        {
            _animator = animator;
            _rigidbody = rigidbody;
            _playerPreferences = staticDataService.Balance.PlayerPreferences;
        }

        public void Tick()
        {
            Vector3 velocity = _rigidbody.velocity;
            Vector2 horizontalVelocity = new Vector2(velocity.x, velocity.z);

            float animationSpeed = horizontalVelocity.magnitude / _playerPreferences.MovementSpeed;
            _animator.SetFloat(_playerPreferences.SpeedParameterName, animationSpeed);
        }
    }
}