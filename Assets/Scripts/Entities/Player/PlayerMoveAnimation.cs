using UnityEngine;

namespace Entities.Player
{
    public class PlayerMoveAnimation : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Player _player;

        [Header("Preferences")]
        [SerializeField] private float _targetSpeed = 5f;
        [SerializeField] private string _speedParameterName = "Speed";

        #region MonoBehaviour

        private void OnValidate()
        {
            _player ??= GetComponentInParent<Player>(true);
        }

        private void Update()
        {
            Vector3 velocity = _player.Rigidbody.velocity;
            Vector2 horizontalVelocity = new Vector2(velocity.x, velocity.z);

            float animationSpeed = horizontalVelocity.magnitude / _targetSpeed;
            _player.Animator.SetFloat(_speedParameterName, animationSpeed);
        }

        #endregion
    }
}