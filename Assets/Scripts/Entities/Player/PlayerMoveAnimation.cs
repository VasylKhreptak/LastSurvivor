using Infrastructure.Data.Static.Balance;
using Infrastructure.Services.StaticData.Core;
using UnityEngine;
using Zenject;

namespace Entities.Player
{
    public class PlayerMoveAnimation : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Player _player;

        [Header("Preferences")]
        [SerializeField] private string _speedParameterName = "Speed";

        private PlayerPreferences _playerPreferences;

        [Inject]
        private void Constructor(IStaticDataService staticDataService)
        {
            _playerPreferences = staticDataService.Balance.PlayerPreferences;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _player ??= GetComponentInParent<Player>(true);
        }

        private void Update()
        {
            Vector3 velocity = _player.Rigidbody.velocity;
            Vector2 horizontalVelocity = new Vector2(velocity.x, velocity.z);

            float animationSpeed = horizontalVelocity.magnitude / _playerPreferences.MovementSpeed;
            _player.Animator.SetFloat(_speedParameterName, animationSpeed);
        }

        #endregion
    }
}