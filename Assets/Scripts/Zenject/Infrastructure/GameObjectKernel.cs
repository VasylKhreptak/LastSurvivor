using Zenject.Infrastructure.Toggleable;

namespace Zenject.Infrastructure
{
    public class GameObjectKernel : MonoKernel
    {
        [InjectLocal]
        private readonly ToggleableManager _toggleableManager;

        #region MonoBehaviour

        private void OnEnable()
        {
            if (HasInitialized == false)
                return;

            _toggleableManager.Enable();
        }

        private void OnDisable()
        {
            _toggleableManager.Disable();
        }

        #endregion

        protected override void OnAfterInitialize()
        {
            _toggleableManager.Enable();
        }
    }
}