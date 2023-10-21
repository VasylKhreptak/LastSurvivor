using Infrastructure.Services.Input.Main.Core;
using UnityEngine;

namespace Infrastructure.Services.Input.Main
{
    public class MainInputService : MonoBehaviour, IMainInputService
    {
        [Header("References")]
        [SerializeField] private Joystick _joystick;

        #region MonoBehaviour

        private void OnValidate()
        {
            _joystick ??= GetComponentInChildren<Joystick>(true);
        }

        #endregion

        public float Horizontal => _joystick.Horizontal;

        public float Vertical => _joystick.Vertical;

        public Vector2 Direction => _joystick.Direction;

        public void Enable()
        {
            gameObject.SetActive(false);
        }

        public void Disable()
        {
            gameObject.SetActive(true);
        }
    }
}