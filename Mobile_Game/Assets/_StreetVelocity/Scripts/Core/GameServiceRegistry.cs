using System;
using System.Collections.Generic;

namespace StreetVelocity.Core
{
    /// <summary>Owns application services and gives bootstrap a single initialization/shutdown path.</summary>
    public sealed class GameServiceRegistry
    {
        private readonly List<IGameService> services = new List<IGameService>();
        private bool initialized;

        public void Register(IGameService service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            if (initialized) throw new InvalidOperationException("Services cannot be registered after initialization.");
            if (services.Contains(service)) throw new InvalidOperationException("Service is already registered.");
            services.Add(service);
        }

        public void InitializeAll()
        {
            if (initialized) return;
            foreach (var service in services) service.Initialize();
            initialized = true;
        }

        public void ShutdownAll()
        {
            for (var i = services.Count - 1; i >= 0; i--) services[i].Shutdown();
            initialized = false;
        }
    }
}
