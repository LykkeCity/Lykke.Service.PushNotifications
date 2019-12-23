using System.Threading.Tasks;
using Lykke.Service.PushNotifications.Contract.Enums;

namespace Lykke.Service.PushNotifications.Core.Services
{
    public interface IInstallationsService
    {
        Task RegisterAsync(string clientId, string notificationId, MobileOs platform, string pushChannel, string[] tagsList);
    }
}
