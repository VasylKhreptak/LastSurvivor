using UnityEngine;

namespace Utilities.TransformUtilities
{
    public class TransformLocalRotator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _transform;

        [Header("Preferences")]
        [SerializeField] private float _speed;
        [SerializeField] private Vector3 _axis;

        #region MonoBehaviour

        private void OnValidate() => _transform ??= GetComponent<Transform>();

        private void Update() => _transform.localRotation *= Quaternion.AngleAxis(_speed * Time.deltaTime, _axis);

        #endregion
    }
}