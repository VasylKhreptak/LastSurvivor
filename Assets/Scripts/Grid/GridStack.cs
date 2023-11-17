using Extensions;
using Flexalon;
using Plugins.Banks;
using UnityEngine;

namespace Grid
{
    public class GridStack
    {
        public readonly FlexalonGridLayout Grid;
        public readonly ClampedIntegerBank Data;

        public GridStack(FlexalonGridLayout grid, ClampedIntegerBank data)
        {
            Grid = grid;
            Data = data;
        }

        public GridStack(FlexalonGridLayout grid, ClampedIntegerBank data, GameObject initialPrefab) : this(grid, data)
        {
            LoadWith(initialPrefab);
        }

        public Transform Root => Grid.transform;

        public bool TryPush(GameObject gameObject)
        {
            if (Data.IsFull.Value)
                return false;

            gameObject.transform.SetParent(Grid.transform, true);
            Grid.ForceUpdate();
            Data.Add(1);
            return true;
        }

        public bool TryPop(out GameObject gameObject)
        {
            if (Data.IsEmpty.Value)
            {
                gameObject = null;
                return false;
            }

            gameObject = Grid.transform.GetChild(Grid.transform.childCount - 1).gameObject;
            gameObject.transform.SetParent(null, true);
            Data.Spend(1);
            Grid.ForceUpdate();
            return true;
        }

        private void LoadWith(GameObject prefab)
        {
            Transform[] children = Grid.transform.GetChildren();

            for (int i = 0; i < children.Length; i++)
            {
                Object.Destroy(children[i].gameObject);
            }

            for (int i = 0; i < Data.Value.Value; i++)
            {
                GameObject gameObject = Object.Instantiate(prefab, Grid.transform);

                gameObject.transform.position = Grid.transform.position;
            }
        }
    }
}