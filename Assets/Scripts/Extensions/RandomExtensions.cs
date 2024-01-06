using UnityEngine;

namespace Extensions
{
    public static class RandomExtensions
    {
        public static float Sign() => Random.value > 0.5f ? 1f : -1f;
    }
}