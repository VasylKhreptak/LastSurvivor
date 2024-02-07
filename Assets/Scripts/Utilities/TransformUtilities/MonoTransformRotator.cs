using UnityEngine;

namespace Utilities.TransformUtilities
{
    public class MonoTransformRotator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _transform;
        [Space]
        [SerializeField] private TransformRotator.Preferences _preferences;

        private TransformRotator _transformRotator;

        #region MonoBehaviour

        private void OnValidate() => _transform ??= GetComponent<Transform>();

        private void Awake() => _transformRotator = new TransformRotator(_transform, _preferences);

        private void Update() => _transformRotator.Tick();

        #endregion
    }
}