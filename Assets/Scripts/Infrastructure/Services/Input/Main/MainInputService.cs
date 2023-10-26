using Infrastructure.Services.Input.Main.Core;
using UniRx;
using UnityEngine;

namespace Infrastructure.Services.Input.Main
{
    public class MainInputService : IMainInputService
    {
        private readonly Joystick _joystick;

        public MainInputService(Joystick joystick)
        {
            _joystick = joystick;
        }

        public float Horizontal => _joystick.Horizontal;

        public float Vertical => _joystick.Vertical;

        public Vector2 Direction => _joystick.Direction;

        public IReadOnlyReactiveProperty<bool> IsInteracting => _joystick.IsPressed;

        public void Enable() => _joystick.gameObject.SetActive(false);

        public void Disable() => _joystick.gameObject.SetActive(true);
    }
}