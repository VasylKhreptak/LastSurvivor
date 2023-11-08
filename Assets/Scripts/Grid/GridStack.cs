using Extensions;
using Flexalon;
using UniRx;
using UnityEngine;

namespace Grid
{
    public class GridStack
    {
        public readonly FlexalonGridLayout Grid;
        private readonly GridData _gridData;

        public GridStack(FlexalonGridLayout grid, GridData gridData)
        {
            Grid = grid;
            _gridData = gridData;
        }

        public IReadOnlyReactiveProperty<int> Count => _gridData.Count;
        public IReadOnlyReactiveProperty<int> Capacity => _gridData.Capacity;
        public IReadOnlyReactiveProperty<bool> IsFull => _gridData.IsFull;
        public IReadOnlyReactiveProperty<bool> IsEmpty => _gridData.IsEmpty;

        public Transform Root => Grid.transform;

        public bool TryPush(GameObject gameObject)
        {
            if (IsFull.Value)
                return false;

            gameObject.transform.SetParent(Grid.transform, true);
            Grid.ForceUpdate();
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

            gameObject = Grid.transform.GetChild(Grid.transform.childCount - 1).gameObject;
            gameObject.transform.SetParent(null, true);
            _gridData.Count.Value--;
            Grid.ForceUpdate();
            return true;
        }

        public void LoadFromGridData(GameObject prefab)
        {
            Transform[] children = Grid.transform.GetChildren();

            for (int i = 0; i < children.Length; i++)
            {
                Object.Destroy(children[i].gameObject);
            }

            for (int i = 0; i < _gridData.Count.Value; i++)
            {
                GameObject gameObject = Object.Instantiate(prefab, Grid.transform);
                gameObject.transform.position = Grid.transform.position;
                gameObject.transform.localScale = Vector3.zero;
            }
        }
    }
}