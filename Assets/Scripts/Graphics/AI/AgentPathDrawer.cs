using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace Graphics.AI
{
    public class AgentPathDrawer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private NavMeshAgent _agent;

        [Header("Preferences")]
        [SerializeField] private Color _color = Color.red;
        [SerializeField] private bool _ifSelectedOnly;

        #region MonoBehaviour

        private void OnValidate() => _agent ??= GetComponent<NavMeshAgent>();

        #endregion

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_ifSelectedOnly && !gameObject.Equals(Selection.activeGameObject))
                return;

            if (_agent == null || _agent.hasPath == false)
                return;

            UnityEngine.Gizmos.color = _color;

            Vector3[] corners = _agent.path.corners;

            for (int i = 0; i < corners.Length - 1; i++)
            {
                UnityEngine.Gizmos.DrawLine(corners[i], corners[i + 1]);
            }
        }
#endif
    }
}