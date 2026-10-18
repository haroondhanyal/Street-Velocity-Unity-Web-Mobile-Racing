namespace StreetVelocity.Vehicle
{
    public interface IVehicleInput
    {
        float Throttle { get; }
        float Steering { get; }
        bool Brake { get; }
        bool Handbrake { get; }
        bool Nitro { get; }
    }
}
