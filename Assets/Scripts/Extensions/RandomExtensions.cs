using UnityEngine;

namespace Extensions
{
    public static class RandomExtensions
    {
        public static float Sign() => Random.value > 0.5f ? 1f : -1f;

        public static Vector3 Range(Vector3 min, Vector3 max) =>
            new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
    }
}