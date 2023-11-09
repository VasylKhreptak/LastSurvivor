using System;
using Zenject;

namespace Platforms.HelicopterPlatform
{
    public class HelicopterUpgrader : IInitializable, IDisposable
    {
        private readonly ReceiveZone _receiveZone;

        public HelicopterUpgrader(ReceiveZone receiveZone)
        {
            _receiveZone = receiveZone;
        }

        public void Initialize() => StartObserving();

        public void Dispose() => StopObserving();

        private void StartObserving() => _receiveZone.OnReceivedAll += Upgrade;

        private void StopObserving() => _receiveZone.OnReceivedAll -= Upgrade;

        private void Upgrade() { }
    }
}