using UnityEngine;

namespace Audio
{
    public class AudioListenerFollower : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private AudioListener _audioListener;
        [SerializeField] private Transform _target;

        private Transform _listenerTransform;

        #region MonoBehaviour

        private void Awake()
        {
            _listenerTransform = _audioListener.transform;

            _listenerTransform.SetParent(null);
            _listenerTransform.rotation = Quaternion.identity;
            _listenerTransform.position = _target.position;
        }

        private void Update() => _listenerTransform.position = _target.position;

        #endregion
    }
}