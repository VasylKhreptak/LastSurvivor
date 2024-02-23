using Grid;
using UnityEngine;
using Zenject;

namespace Main.Entities.Player
{
    public class Player : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _inputOutputTransform;
        [SerializeField] private QuestArrow _questArrow;

        [Inject]
        private void Constructor(Transform transform, GridStack gridStack)
        {
            Transform = transform;
            BarrelGridStack = gridStack;
        }

        public Transform Transform { get; private set; }
        public GridStack BarrelGridStack { get; private set; }
        public Transform InputOutputTransform => _inputOutputTransform;
        public QuestArrow QuestArrow => _questArrow;
    }
}