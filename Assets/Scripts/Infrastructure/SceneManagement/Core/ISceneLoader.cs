using System;

namespace Infrastructure.SceneManagement.Core
{
    public interface ISceneLoader
    {
        public void Load(string name);

        public void LoadAsync(string name, Action onComplete = null);

        public void ReloadScene();

        public void ReloadSceneAsync(Action onComplete = null);
    }
}