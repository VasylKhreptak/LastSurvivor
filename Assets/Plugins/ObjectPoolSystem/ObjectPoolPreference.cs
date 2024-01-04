using System;
using UnityEngine;

namespace Plugins.ObjectPoolSystem
{
    [Serializable]
    public class ObjectPoolPreference<T> where T : Enum
    {
        public T Key;
        public Func<GameObject> CreateFunc;
        public GameObject Prefab;
        public int InitialSize;
        public int MaxSize;
    }
}
