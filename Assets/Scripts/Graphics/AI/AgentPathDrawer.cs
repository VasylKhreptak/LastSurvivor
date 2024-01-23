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

        private void OnDrawGizmos()
        {
            if (_ifSelectedOnly && !gameObject.Equals(Selection.activeGameObject))
                return;

            if (_agent.hasPath == false)
                return;

            UnityEngine.Gizmos.color = _color;

            for (int i = 0; i < _agent.path.corners.Length - 1; i++)
            {
                UnityEngine.Gizmos.DrawLine(_agent.path.corners[i], _agent.path.corners[i + 1]);
            }
        }
    }
}