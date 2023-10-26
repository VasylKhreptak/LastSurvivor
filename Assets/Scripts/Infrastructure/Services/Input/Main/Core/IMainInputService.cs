using UniRx;
using UnityEngine;

namespace Infrastructure.Services.Input.Main.Core
{
    public interface IMainInputService
    {
        public float Horizontal { get; }

        public float Vertical { get; }

        public Vector2 Direction { get; }

        public IReadOnlyReactiveProperty<bool> IsInteracting { get; }

        public void Enable();

        public void Disable();
    }
}