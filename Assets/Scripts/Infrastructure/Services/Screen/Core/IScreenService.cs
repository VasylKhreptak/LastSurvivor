using UniRx;
using UnityEngine;

namespace Infrastructure.Services.Screen.Core
{
    public interface IScreenService
    {
        public IReadOnlyReactiveProperty<ScreenOrientation> ScreenOrientation { get; }
        public IReadOnlyReactiveProperty<Vector2Int> ScreenResolution { get; }
    }
}