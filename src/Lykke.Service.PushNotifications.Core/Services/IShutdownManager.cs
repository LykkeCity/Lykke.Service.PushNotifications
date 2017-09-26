using System.Threading.Tasks;

namespace Lykke.Service.PushNotifications.Core.Services
{
    public interface IShutdownManager
    {
        Task StopAsync();
    }
}