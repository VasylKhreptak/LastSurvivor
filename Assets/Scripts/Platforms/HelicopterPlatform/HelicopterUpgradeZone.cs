using UnityEngine;

namespace Platforms.HelicopterPlatform
{
    public class HelicopterUpgradeZone : ReceiveZone
    {
        protected override void OnReceivedAll()
        {
            Debug.Log("OnAllTransferred");
        }
    }
}