using System.Threading.Tasks;

namespace Lykke.Service.PushNotifications.Core.Domain
{
    public interface IFcmTokensRepository
    {
        Task AddAsync(string notificationId, string clientId, string sessionId, string token);
        Task<string[]> GetAsync(string notificationId);
        Task<string[]> GetAsync(string[] notificationIds);
        Task DeleteBySessionIdAsync(string sessionId);
    }
}
