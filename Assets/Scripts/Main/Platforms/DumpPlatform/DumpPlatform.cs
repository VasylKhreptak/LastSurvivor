using Main.Platforms.Zones;
using UnityEngine;
using Zenject;

namespace Main.Platforms.DumpPlatform
{
    public class DumpPlatform : MonoBehaviour
    {
        private ReceiveZone _hireWorkerZone;

        [Inject]
        private void Constructor(ReceiveZone receiveZone)
        {
            _hireWorkerZone = receiveZone;
        }

        public ReceiveZone HireWorkerZone => _hireWorkerZone;
    }
}