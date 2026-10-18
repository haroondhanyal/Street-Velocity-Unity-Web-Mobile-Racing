using System.Threading.Tasks;

namespace StreetVelocity.UI
{
    public interface IScreenNavigator { Task ShowAsync(string screenId); }
}
