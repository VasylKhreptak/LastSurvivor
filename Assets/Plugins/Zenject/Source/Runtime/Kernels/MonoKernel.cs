#if !NOT_UNITY3D

#pragma warning disable 649

using ModestTree;
using UnityEngine;

namespace Zenject
{
    public abstract class MonoKernel : MonoBehaviour
    {
        [InjectLocal] private readonly TickableManager _tickableManager;

        [InjectLocal] private readonly InitializableManager _initializableManager;

        [InjectLocal] private readonly DisposableManager _disposablesManager;

        [InjectOptional] private readonly IDecoratableMonoKernel decoratableMonoKernel;

        protected bool IsDestroyed { get; private set; }

        protected bool HasInitialized { get; private set; }

        public virtual void Start()
        {
            if (decoratableMonoKernel?.ShouldInitializeOnStart() ?? true)
                Initialize();
        }

        public void Initialize()
        {
            if (!HasInitialized)
            {
                HasInitialized = true;

                if (decoratableMonoKernel != null)
                    decoratableMonoKernel.Initialize();
                else
                    _initializableManager.Initialize();

                OnAfterInitialize();
            }
        }

        protected virtual void OnAfterInitialize() { }

        public virtual void Update()
        {
            if (_tickableManager != null)
            {
                if (decoratableMonoKernel != null)
                    decoratableMonoKernel.Update();
                else
                    _tickableManager.Update();
            }
        }

        public virtual void FixedUpdate()
        {
            if (_tickableManager != null)
            {
                if (decoratableMonoKernel != null)
                    decoratableMonoKernel.FixedUpdate();
                else
                    _tickableManager.FixedUpdate();
            }
        }

        public virtual void LateUpdate()
        {
            if (_tickableManager != null)
            {
                if (decoratableMonoKernel != null)
                    decoratableMonoKernel.LateUpdate();
                else
                    _tickableManager.LateUpdate();
            }
        }

        public virtual void OnDestroy()
        {
            if (_disposablesManager != null)
            {
                Assert.That(!IsDestroyed);
                IsDestroyed = true;

                if (decoratableMonoKernel != null)
                {
                    decoratableMonoKernel.Dispose();
                    decoratableMonoKernel.LateDispose();
                }
                else
                {
                    _disposablesManager.Dispose();
                    _disposablesManager.LateDispose();
                }
            }
        }
    }
}

#endif