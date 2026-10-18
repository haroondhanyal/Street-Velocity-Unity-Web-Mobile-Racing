namespace StreetVelocity.Core
{
    /// <summary>Lifecycle contract for application-wide services.</summary>
    public interface IGameService
    {
        void Initialize();
        void Shutdown();
    }
}
