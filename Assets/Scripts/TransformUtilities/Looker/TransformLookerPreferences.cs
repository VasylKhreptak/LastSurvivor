using UnityEngine;

namespace TransformUtilities.Looker
{
    public class TransformLookerPreferences
    {
        public Transform Source;
        public Transform Target;
        public Vector3 Upwards = Vector3.up;
        public float RotationSpeed = 1f;
    }
}