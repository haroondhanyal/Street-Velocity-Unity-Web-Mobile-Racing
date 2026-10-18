using System.Threading.Tasks;

namespace StreetVelocity.UI
{
    public interface IUIScreen
    {
        string ScreenId { get; }
        Task ShowAsync();
        Task HideAsync();
    }
}
