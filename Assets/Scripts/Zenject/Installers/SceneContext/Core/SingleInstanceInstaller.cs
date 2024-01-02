using UnityEngine;

namespace Zenject.Installers.SceneContext.Core
{
    public class SingleInstanceInstaller<T> : MonoInstaller where T : Object
    {
        [Header("References")]
        [SerializeField] private T _instance;

        #region MonoBehaviour

        private void OnValidate() => _instance ??= FindObjectOfType<T>();

        public override void InstallBindings() => Container.BindInstance(_instance).AsSingle();

        #endregion
    }
}