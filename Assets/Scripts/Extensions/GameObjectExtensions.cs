using UnityEngine;

namespace Extensions
{
    public static class GameObjectExtensions
    {
        public static T TryAddComponent<T>(this GameObject gameObject) where T : Component
        {
            if (gameObject.TryGetComponent(out T existingComponent))
                return existingComponent;

            return gameObject.AddComponent<T>();
        }
    }
}