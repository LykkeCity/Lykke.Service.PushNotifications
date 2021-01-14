using System.Threading.Tasks;
using Lykke.Service.PushNotifications.Client.Models;
using Refit;

namespace Lykke.Service.PushNotifications.Client
{
    public interface IFcmTokensApi
    {
        [Post("/api/fcmtokens")]
        Task RegisterAsync([Body] FcmTokenModel model);
    }
}
