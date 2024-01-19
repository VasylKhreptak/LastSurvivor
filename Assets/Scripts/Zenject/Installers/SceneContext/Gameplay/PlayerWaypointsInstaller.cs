using Extensions;
using Gameplay.Waypoints;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Zenject.Installers.SceneContext.Gameplay
{
    public class PlayerWaypointsInstaller : MonoInstaller
    {
        [SerializeField] private Transform[] _waypoints;

        public override void InstallBindings()
        {
            Container.Bind<PlayerWaypoints>().AsSingle().WithArguments(_waypoints);
        }

        [Button]
        private void Sync() => _waypoints = transform.GetChildren();
    }
}