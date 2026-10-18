using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace StreetVelocity.Core
{
    public interface ISceneLoader { Task LoadAsync(string sceneName); }

    public sealed class SceneLoader : ISceneLoader
    {
        public async Task LoadAsync(string sceneName)
        {
            if (string.IsNullOrWhiteSpace(sceneName)) throw new ArgumentException("Scene name is required.", nameof(sceneName));
            var operation = SceneManager.LoadSceneAsync(sceneName);
            if (operation == null) throw new InvalidOperationException($"Could not load scene '{sceneName}'. Is it in Build Settings?");
            while (!operation.isDone) await Task.Yield();
        }
    }
}
