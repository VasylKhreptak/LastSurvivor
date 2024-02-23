using UnityEngine;

namespace Main.Entities.Player
{
    public class QuestArrow : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _arrow;

        [Header("Preferences")]
        [SerializeField] private float _distanceThreshold = 2f;

        [HideInInspector] public Transform Target;

        private Vector3 _lookPoint;
        private Vector3 _position;

        #region MonoBehaviour

        private void Update()
        {
            _position = transform.position;

            _lookPoint = Target.position;
            _lookPoint.y = _position.y;

            if (Target == null || Vector3.Distance(_lookPoint, _position) < _distanceThreshold)
            {
                _arrow.gameObject.SetActive(false);
                return;
            }

            _arrow.gameObject.SetActive(true);
            _arrow.rotation = Quaternion.LookRotation(_lookPoint - _position);
        }

        #endregion

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _distanceThreshold);

            if (Target == null)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, Target.position);
        }
    }
}