using UnityEngine;

namespace ObjectPoolSystem
{
    public class MonoLifetimeHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _gameObject;

        [Space]
        [SerializeField] private LifetimeHandler.Preferences _preferences;

        private LifetimeHandler _lifetimeHandler;

        #region MonoBehaviour

        private void OnValidate() => _gameObject ??= GetComponent<GameObject>();

        private void Awake() => _lifetimeHandler = new LifetimeHandler(_gameObject, _preferences);

        private void OnEnable() => _lifetimeHandler.Enable();

        private void OnDisable() => _lifetimeHandler.Disable();

        #endregion
    }
}