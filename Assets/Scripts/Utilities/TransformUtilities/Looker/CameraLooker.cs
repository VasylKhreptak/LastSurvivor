using UnityEngine;
using Zenject;

namespace Utilities.TransformUtilities.Looker
{
    public class CameraLooker : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _transform;

        [Header("Preferences")]
        [SerializeField] private Vector3 _upwards = Vector3.up;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _lookSpeed = 15f;

        private Transform _cameraTransform;

        [Inject]
        private void Constructor(Camera camera)
        {
            _cameraTransform = camera.transform;
        }

        private TransformLooker _looker;

        #region MonoBehaviour

        private void OnValidate() => _transform ??= GetComponent<Transform>();

        private void Awake() => Initialize();

        private void OnEnable() => _looker.LookImmediately();

        private void Update() => _looker.Tick();

        #endregion

        private void Initialize()
        {
            TransformLookerPreferences preferences = new TransformLookerPreferences
            {
                Source = _transform,
                Target = _cameraTransform,
                Upwards = _upwards,
                Offset = _offset,
                LookSpeed = _lookSpeed
            };

            _looker = new TransformLooker(preferences);
        }
    }
}