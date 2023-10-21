using Infrastructure.Services.Input.Main.Core;
using UnityEngine;
using Zenject;

namespace Entities.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Player _player;

        [Header("Preferences")]
        [SerializeField] private float _speed = 5f;

        private IMainInputService _mainInputService;

        [Inject]
        private void Constructor(IMainInputService mainInputService)
        {
            _mainInputService = mainInputService;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _player ??= GetComponentInParent<Player>(true);
        }

        private void Update() => HandleMovement();

        #endregion

        private void HandleMovement()
        {
            Vector2 input = _mainInputService.Direction;

            if (Mathf.Approximately(input.magnitude, 0f))
                return;

            Vector3 moveDirection = new Vector3(input.x, 0f, input.y);
            Vector3 verticalVelocity = Vector3.up * _player.Rigidbody.velocity.y;

            _player.Transform.forward = moveDirection;
            _player.Rigidbody.velocity = moveDirection * _speed + verticalVelocity;
        }
    }
}