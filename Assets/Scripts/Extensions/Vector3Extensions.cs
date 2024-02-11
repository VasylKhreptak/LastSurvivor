using UnityEngine;

namespace Extensions
{
    public static class Vector3Extensions
    {
        public static bool IsCloseTo(this Vector3 a, Vector3 b, float threshold) => Vector3.Distance(a, b) < threshold;
    }
}