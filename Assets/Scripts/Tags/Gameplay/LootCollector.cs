using UnityEngine;

namespace Tags.Gameplay
{
    public class LootCollector : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _target;

        public Transform Target => _target;
    }
}