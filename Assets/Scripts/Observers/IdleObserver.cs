using UniRx;
using UnityEngine;
using Zenject;

namespace Observers
{
    public class IdleObserver : ITickable
    {
        private const float TimeToIdle = 10f;

        private float _time;

        private readonly BoolReactiveProperty _isIdling = new BoolReactiveProperty(false);

        public IReadOnlyReactiveProperty<bool> IsIdling => _isIdling;

        public void Tick()
        {
            if (Input.touchCount > 0)
                _time = 0;
            else
                _time += Time.deltaTime;

            _isIdling.Value = _time >= TimeToIdle;
        }
    }
}