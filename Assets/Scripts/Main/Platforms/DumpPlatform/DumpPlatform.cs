using Main.Platforms.Zones;
using UnityEngine;
using Zenject;

namespace Main.Platforms.DumpPlatform
{
    public class DumpPlatform : MonoBehaviour
    {
        [Inject]
        private void Constructor(ReceiveZone receiveZone)
        {
            HireWorkerZone = receiveZone;
        }

        public ReceiveZone HireWorkerZone { get; private set; }
    }
}