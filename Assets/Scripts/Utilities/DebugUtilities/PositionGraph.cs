using UnityEngine;

namespace Utilities.DebugUtilities
{
    public class PositionGraph : MonoBehaviour
    {
#if UNITY_EDITOR

        [SerializeField] private AnimationCurve _x;
        [SerializeField] private AnimationCurve _y;
        [SerializeField] private AnimationCurve _z;

        #region MonoBehaviour

        private void Update()
        {
            UpdateCurve();
            RemoveOlderThan(10f);
            MoveToLeft();
        }

        private void UpdateCurve()
        {
            Vector3 position = transform.position;
            _x.AddKey(new Keyframe(Time.time, position.x, 0f, 0f, 0f, 0f));
            _y.AddKey(new Keyframe(Time.time, position.y, 0f, 0f, 0f, 0f));
            _z.AddKey(new Keyframe(Time.time, position.z, 0f, 0f, 0f, 0f));
        }

        private void RemoveOlderThan(float time)
        {
            for (int i = 0; i < _x.length; i++)
            {
                if (_x[i].time < Time.time - 10)
                {
                    _x.RemoveKey(i);
                    _y.RemoveKey(i);
                    _z.RemoveKey(i);
                }
            }
        }

        private void MoveToLeft()
        {
            for (int i = 0; i < _x.length; i++)
            {
                Keyframe key = _x[i];
                key.time -= Time.deltaTime;
                _x.MoveKey(i, key);

                key = _y[i];
                key.time -= Time.deltaTime;
                _y.MoveKey(i, key);

                key = _z[i];
                key.time -= Time.deltaTime;
                _z.MoveKey(i, key);
            }
        }

        #endregion

#endif
    }
}