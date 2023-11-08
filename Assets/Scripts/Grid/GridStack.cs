using Extensions;
using Flexalon;
using UniRx;
using UnityEngine;

namespace Grid
{
    public class GridStack
    {
        private readonly FlexalonGridLayout _grid;
        private readonly GridData _gridData;

        public GridStack(FlexalonGridLayout grid, GridData gridData)
        {
            _grid = grid;
            _gridData = gridData;
        }

        public IReadOnlyReactiveProperty<int> Count => _gridData.Count;
        public IReadOnlyReactiveProperty<int> Capacity => _gridData.Capacity;
        public IReadOnlyReactiveProperty<bool> IsFull => _gridData.IsFull;
        public IReadOnlyReactiveProperty<bool> IsEmpty => _gridData.IsEmpty;

        public Transform Root => _grid.transform;

        public bool TryPush(GameObject gameObject)
        {
            if (IsFull.Value)
                return false;

            gameObject.transform.SetParent(_grid.transform, true);
            _gridData.Count.Value++;
            return true;
        }

        public bool TryPop(out GameObject gameObject)
        {
            if (IsEmpty.Value)
            {
                gameObject = null;
                return false;
            }

            gameObject = _grid.transform.GetChild(_grid.transform.childCount - 1).gameObject;
            gameObject.transform.SetParent(null, true);
            _gridData.Count.Value--;
            return true;
        }

        public void LoadFromGridData(GameObject prefab)
        {
            Transform[] children = _grid.transform.GetChildren();

            for (int i = 0; i < children.Length; i++)
            {
                Object.Destroy(children[i].gameObject);
            }

            for (int i = 0; i < _gridData.Count.Value; i++)
            {
                GameObject gameObject = Object.Instantiate(prefab, _grid.transform);
                gameObject.transform.position = _grid.transform.position;
                gameObject.transform.localScale = Vector3.zero;
            }
        }
    }
}