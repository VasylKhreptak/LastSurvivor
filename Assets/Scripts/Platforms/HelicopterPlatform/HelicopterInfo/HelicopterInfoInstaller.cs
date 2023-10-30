using Infrastructure.Data.Static;
using Infrastructure.Services.StaticData.Core;
using TransformUtilities.Looker;
using UnityEngine;
using Zenject;

namespace Platforms.HelicopterPlatform.HelicopterInfo
{
    public class HelicopterInfoInstaller : MonoInstaller
    {
        [Header("References")]
        [SerializeField] private Transform _rootTransform;

        private Camera _camera;
        private GameBalance _balance;

        [Inject]
        private void Constructor(Camera camera, IStaticDataService staticDataService)
        {
            _camera = camera;
            _balance = staticDataService.Balance;
        }

        public override void InstallBindings()
        {
            BindTransformLooker();
        }

        private void BindTransformLooker()
        {
            Container.Bind<TransformLookerPreferences>()
                .FromInstance(new TransformLookerPreferences
                {
                    Source = _rootTransform,
                    Target = _camera.transform,
                    Upwards = Vector3.up,
                    Offset = _balance.TransformLookerPreferences.RotationOffsetForUI,
                    LookSpeed = _balance.TransformLookerPreferences.DefaultLookSpeed
                })
                .AsSingle();

            Container.BindInterfacesTo<TransformLooker>().AsSingle();
        }
    }
}