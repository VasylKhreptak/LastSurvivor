using System;
using Random = UnityEngine.Random;

namespace Protection.SafeVariables
{
    public struct SafeInt
    {
        private int _value;
        private int _salt;

        public SafeInt(int value)
        {
            _salt = Random.Range(int.MinValue / 4, int.MaxValue / 4);
            _value = value ^ _salt;
        }

        public override bool Equals(object obj) => this == (int)obj;

        public override int GetHashCode() => base.GetHashCode();

        public override string ToString() => ((int)this).ToString();

        public static implicit operator int(SafeInt safeInt) => safeInt._value ^ safeInt._salt;

        public static implicit operator SafeInt(int normalInt) => new SafeInt(normalInt);
    }
}