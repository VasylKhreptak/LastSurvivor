using System;

namespace Serialization.Collections.KeyValue
{
    [Serializable]
    public class KeyValuePair<TKey, TValue>
    {
        public TKey Key;
        public TValue Value;

        public KeyValuePair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}