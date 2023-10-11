using UnityEngine;
using Zenject;

namespace Infrastructure.EntryPoints.Game
{
    public class GameRunner : MonoBehaviour
    {
        private void Awake()
        {
            if (FindObjectOfType<ProjectContext>() == null)
            {
                GameObject sceneContextObject = new GameObject("SceneContext");
                sceneContextObject.AddComponent<SceneContext>();
            }

            Destroy(gameObject);
        }
    }
}
