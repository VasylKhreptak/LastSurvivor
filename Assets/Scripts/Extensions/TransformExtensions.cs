using UnityEngine;

namespace Extensions
{
    public static class TransformExtensions
    {
        public static Transform[] GetChildren(this Transform transform)
        {
            Transform[] children = new Transform[transform.childCount];

            for (int i = 0; i < transform.childCount; i++)
            {
                children[i] = transform.GetChild(i);
            }

            return children;
        }

        public static Transform CreateParent(this Transform transform, string name)
        {
            Transform parent = new GameObject(name).transform;
            Transform childParent = transform.parent;
            parent.SetParent(childParent);
            parent.localPosition = transform.localPosition;
            parent.localRotation = transform.localRotation;
            parent.localScale = transform.localScale;
            transform.SetParent(parent, true);
            // transform.localPosition = Vector3.zero;
            // transform.localRotation = Quaternion.identity;
            // transform.localScale = Vector3.one;
            return parent;
        }
    }
}