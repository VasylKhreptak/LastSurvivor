using UnityEngine;

namespace Extensions
{
    public static class AnimationCurveExtensions
    {
        public static float Evaluate(this AnimationCurve curve, float min, float max, float i) =>
            curve.Evaluate(i) * (max - min) + min;

        public static float Evaluate(this AnimationCurve curve, float minIn, float maxIn, float @in, float minOut, float maxOut)
        {
            float remappedIn = @in.Remap(minIn, maxIn, 0f, 1f);

            return curve.Evaluate(minOut, maxOut, remappedIn);
        }
    }
}