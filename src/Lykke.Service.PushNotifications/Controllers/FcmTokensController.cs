using System.Net;
using System.Threading.Tasks;
using Lykke.Service.PushNotifications.Client;
using Lykke.Service.PushNotifications.Client.Models;
using Lykke.Service.PushNotifications.Core.Domain;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Lykke.Service.PushNotifications.Controllers
{
    [Route("api/fcmtokens")]
    public class FcmTokensController : Controller, IFcmTokensApi
    {
        private readonly IFcmTokensRepository _fcmTokensRepository;

        public FcmTokensController(
            IFcmTokensRepository fcmTokensRepository
            )
        {
            _fcmTokensRepository = fcmTokensRepository;
        }

        [HttpPost]
        [SwaggerOperation("RegisterFcmToken")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task RegisterAsync([FromBody] FcmTokenModel model)
        {
            return _fcmTokensRepository.AddAsync(model.NotificationId, model.ClientId, model.SessionId, model.FcmToken);
        }

        [HttpDelete("{sessionId}")]
        [SwaggerOperation("RemoveFcmTokensBySession")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task RemoveAsync(string sessionId)
        {
            return _fcmTokensRepository.DeleteBySessionIdAsync(sessionId);
        }
    }
}
