using System.Threading.Tasks;

namespace Lykke.Service.PushNotifications.Core.Services
{
    public interface IStartupManager
    {
        Task StartAsync();
    }
}