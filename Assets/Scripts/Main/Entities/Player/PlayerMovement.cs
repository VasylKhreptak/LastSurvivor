using Data.Static.Balance;
using Infrastructure.Services.Input.Main.Core;
using UnityEngine;
using Zenject;

namespace Main.Entities.Player
{
    public class PlayerMovement : ITickable
    {
        private readonly Transform _transform;
        private readonly Animator _animator;
        private readonly CharacterController _characterController;
        private readonly PlayerPreferences _playerPreferences;
        private readonly IMainInputService _inputService;

        public PlayerMovement(Transform transform, Animator animator, CharacterController characterController,
            PlayerPreferences playerPreferences, IMainInputService mainInputService)
        {
            _transform = transform;
            _animator = animator;
            _characterController = characterController;
            _playerPreferences = playerPreferences;
            _inputService = mainInputService;
        }

        private Vector3 _currentDirection;
        private Vector3 _targetDirection;
        private Vector3 _intermediateDirection;
        private Quaternion _targetRotation;

        private float _joystickMagnitude;
        private float _speed;

        private Vector3 _gravityAffectedVelocity;

        public void Tick()
        {
            HandleRotation();
            HandleMovement();
            HandleGravity();
        }

        private void HandleRotation()
        {
            _currentDirection = _transform.forward;
            _targetDirection = new Vector3(_inputService.Horizontal, 0, _inputService.Vertical);

            if (_targetDirection == Vector3.zero)
                return;

            _intermediateDirection =
                Vector3.Lerp(_currentDirection, _targetDirection, _playerPreferences.RotateSpeed * Time.deltaTime);
            _targetRotation = Quaternion.LookRotation(_intermediateDirection);
            _transform.rotation = _targetRotation;
        }

        private void HandleMovement()
        {
            _joystickMagnitude = _inputService.Direction.magnitude;
            _speed = Mathf.Lerp(_speed, _joystickMagnitude, _playerPreferences.Acceleration * Time.deltaTime);
            _animator.SetFloat(_playerPreferences.SpeedParameterName, _speed);
        }

        private void HandleGravity()
        {
            if (_characterController.isGrounded)
            {
                _gravityAffectedVelocity = Vector3.zero;
                return;
            }

            _gravityAffectedVelocity += Physics.gravity * _playerPreferences.Gravity * Time.deltaTime;
            _characterController.Move(_gravityAffectedVelocity);
        }
    }
}