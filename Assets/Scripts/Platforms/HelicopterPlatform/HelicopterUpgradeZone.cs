using UnityEngine;

namespace Platforms.HelicopterPlatform
{
    public class HelicopterUpgradeZone : TransferZone
    {
        protected override void OnAllTransferred()
        {
            Debug.Log("OnAllTransferred");
        }
    }
}