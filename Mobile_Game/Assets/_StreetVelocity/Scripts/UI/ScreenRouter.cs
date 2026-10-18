using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace StreetVelocity.UI
{
    /// <summary>Scene-local registry; screens are registered explicitly by their composition root.</summary>
    public sealed class ScreenRouter : MonoBehaviour, IScreenNavigator
    {
        [SerializeField] private MonoBehaviour[] screenComponents;
        [SerializeField] private string initialScreenId;
        private readonly Dictionary<string, IUIScreen> screens = new Dictionary<string, IUIScreen>();
        private IUIScreen active;

        private async void Start()
        {
            foreach (var component in screenComponents)
            {
                if (component is IUIScreen screen) Register(screen);
                else if (component != null) Debug.LogError($"{component.name} does not implement IUIScreen.", component);
            }
            if (!string.IsNullOrWhiteSpace(initialScreenId)) await ShowAsync(initialScreenId);
        }

        public void Register(IUIScreen screen)
        {
            if (screen == null || string.IsNullOrWhiteSpace(screen.ScreenId)) throw new ArgumentException("A screen and non-empty ID are required.");
            if (screens.ContainsKey(screen.ScreenId)) throw new InvalidOperationException($"Screen '{screen.ScreenId}' is already registered.");
            screens.Add(screen.ScreenId, screen);
        }

        public async Task ShowAsync(string screenId)
        {
            if (!screens.TryGetValue(screenId, out var next)) throw new KeyNotFoundException($"Screen '{screenId}' is not registered.");
            if (active == next) return;
            if (active != null) await active.HideAsync();
            active = next;
            await active.ShowAsync();
        }
    }
}
