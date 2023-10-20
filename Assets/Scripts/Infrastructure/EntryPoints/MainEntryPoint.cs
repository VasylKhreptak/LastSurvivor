using Infrastructure.EntryPoints.Core;
using UnityEngine;

namespace Infrastructure.EntryPoints
{
    public class MainEntryPoint : MonoBehaviour, IEntryPoint
    {
        #region MonoBehaviour

        private void Start()
        {
            Enter();
        }

        #endregion

        public void Enter() { }
    }
}