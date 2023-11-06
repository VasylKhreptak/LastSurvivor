#if UNITY_PHYSICS

using UnityEngine;

namespace Flexalon
{
    /// <summary> A drag target allows a layout to accept  dragged FlexalonInteractable objects. </summary>
    [AddComponentMenu("Flexalon/Flexalon Drag Target"), HelpURL("https://www.flexalon.com/docs/dragging"), DisallowMultipleComponent]
    public class FlexalonDragTarget : MonoBehaviour
    {
        [SerializeField]
        private bool _canRemoveObjects = true;
        /// <summary> Whether objects can be removed from the layout by dragging them from this target. </summary>
        public bool CanRemoveObjects {
            get => _canRemoveObjects;
            set => _canRemoveObjects = value;
        }

        [SerializeField]
        private bool _canAddObjects = true;
        /// <summary> Whether objects can be added to the layout by dragging them to this target. </summary>
        public bool CanAddObjects {
            get => _canAddObjects;
            set => _canAddObjects = value;
        }

        [SerializeField]
        private int _minObjects;
        /// <summary> The minimum number of objects that must remain in this layout. </summary>
        public int MinObjects {
            get => _minObjects;
            set => _minObjects = value;
        }

        [SerializeField]
        private int _maxObjects;
        /// <summary> The maximum number of objects that can be added to the layout. </summary>
        public int MaxObjects {
            get => _maxObjects;
            set => _maxObjects = value;
        }

        [SerializeField]
        private Vector3 _margin;
        /// <summary> Extra margin around the layout size to use for hit testing. </summary>
        public Vector3 Margin {
            get => _margin;
            set
            {
                _margin = value;
                if (_node != null)
                {
                    OnResultChanged(_node);
                }
            }
        }

        private BoxCollider _collider;
        private FlexalonNode _node;

        void OnEnable()
        {
            _node = Flexalon.GetOrCreateNode(gameObject);
            _collider = gameObject.AddComponent<BoxCollider>();
            _node.ResultChanged += OnResultChanged;
            OnResultChanged(_node);
        }

        void OnDisable()
        {
            _node.ResultChanged -= OnResultChanged;
            Destroy(_collider);
            _collider = null;
            _node = null;
        }

        void OnResultChanged(FlexalonNode node)
        {
            if (_collider)
            {
                _collider.center = node.Result.LayoutBounds.center;
                _collider.size = node.Result.LayoutBounds.size + _margin;
            }
        }
    }
}

#endif