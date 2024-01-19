using System;
using System.Collections.Generic;
using UnityEngine;

namespace Serialization.Collections.KeyValue
{
    [Serializable]
    public class KeyValuePairs<TKey, TValue>
    {
        [SerializeField] private List<KeyValuePair<TKey, TValue>> _pairs = new List<KeyValuePair<TKey, TValue>>();

        public Dictionary<TKey, TValue> ToDictionary()
        {
            Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

            foreach (KeyValuePair<TKey, TValue> pair in _pairs)
            {
                dictionary.Add(pair.Key, pair.Value);
            }

            return dictionary;
        }
    }
}