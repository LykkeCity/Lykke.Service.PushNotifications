using System.Threading.Tasks;
using Lykke.Service.PushNotifications.Contract.Enums;

namespace Lykke.Service.PushNotifications.Core.Services
{
    public interface IInstallationsService
    {
        Task<string> RegisterAsync(string clientId, string installationId, string notificationId, MobileOs platform, string pushChannel);
    }
}
