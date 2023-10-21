using Plugins.Animations.Extensions;
using UnityEngine;

namespace Graphics.Gizmos
{
    public class GizmosSphereDrawer : MonoBehaviour
    {
        [Header("Preferences")]
        [SerializeField] private Color _color = Color.red.WithAlpha(0.5f);
        [SerializeField, Min(0)] private float _range = 0.2f;
        [SerializeField] private bool _drawOnlyWhenSelected = true;

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            if (_drawOnlyWhenSelected && UnityEditor.Selection.Contains(gameObject) == false)
                return;

            UnityEngine.Gizmos.color = _color;
            UnityEngine.Gizmos.DrawSphere(transform.position, _range);
        }

#endif
    }
}