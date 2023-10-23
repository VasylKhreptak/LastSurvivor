using System.Collections.Generic;
using Zenject.Infrastructure.Toggleable.Core;

namespace Zenject.Infrastructure.Toggleable
{
    public class ToggleableManager : IEnableable, IDisableable
    {
        [Inject(Optional = true, Source = InjectSources.Local)]
        private readonly List<IEnableable> _enableables = new List<IEnableable>();

        [Inject(Optional = true, Source = InjectSources.Local)]
        private readonly List<IDisableable> _disableables = new List<IDisableable>();

        private bool _isEnabled;

        public void Add(IEnableable enableable)
        {
            _enableables.Add(enableable);
        }

        public void Remove(IEnableable enableable)
        {
            _enableables.Remove(enableable);
        }

        public void Add(IDisableable disableable)
        {
            _disableables.Add(disableable);
        }

        public void Remove(IDisableable disableable)
        {
            _disableables.Remove(disableable);
        }

        public void Enable()
        {
            if (!_isEnabled)
            {
                foreach (IEnableable enableable in _enableables)
                {
                    enableable.Enable();
                }

                _isEnabled = true;
            }
        }

        public void Disable()
        {
            if (_isEnabled)
            {
                foreach (IDisableable disableable in _disableables)
                {
                    disableable.Disable();
                }

                _isEnabled = false;
            }
        }
    }
}