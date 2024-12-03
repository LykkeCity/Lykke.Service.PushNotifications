using System.Threading.Tasks;

namespace Lykke.Service.PushNotifications.Core.Services
{
    public interface IFirebasePushService
    {
        Task SendPushNotifications(string[] notificationsIds, string title, string message);
    }
}
