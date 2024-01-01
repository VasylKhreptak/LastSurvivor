using Infrastructure.EntryPoints.Core;
using UnityEngine;

namespace Infrastructure.EntryPoints
{
    public class TutorialEntryPoint : MonoBehaviour, IEntryPoint
    {
        #region MonoBehaviour

        private void Start() => Enter();

        #endregion

        public void Enter()
        {
            Debug.Log("Enter Tutorial");
        }
    }
}