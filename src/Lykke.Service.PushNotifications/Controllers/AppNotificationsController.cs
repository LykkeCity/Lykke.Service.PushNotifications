using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Service.PushNotifications.Client;
using Lykke.Service.PushNotifications.Client.Models;
using Lykke.Service.PushNotifications.Contract.Enums;
using Microsoft.AspNetCore.Mvc;
using Lykke.Service.PushNotifications.Core.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace Lykke.Service.PushNotifications.Controllers
{
    [Route("api/AppNotifications")]
    public class AppNotificationsController:  Controller, INotificationsApi
    {
        private readonly IAppNotifications _appNotifications;

        public AppNotificationsController(
            IAppNotifications appNotifications
            )
        {
            _appNotifications = appNotifications;
        }

        [HttpPost("SendDataNotificationToAllDevices")]
        [SwaggerOperation("SendDataNotificationToAllDevices")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public Task SendDataNotificationToAllDevicesAsync([FromBody] DataNotificationModel model)
        {
            return _appNotifications.SendDataNotificationToAllDevicesAsync(model.NotificationIds.ToArray(), model.Type, model.Entity, model.Id);
        }

        [HttpPost("SendTextNotification")]
        [SwaggerOperation("SendTextNotification")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public Task SendTextNotificationAsync([FromBody] TextNotificationModel model)
        {
            return _appNotifications.SendTextNotificationAsync(model.NotificationIds.ToArray(), model.Type,
                model.Message);
        }

        [HttpPost("SendPushTxDialog")]
        [SwaggerOperation("SendPushTxDialog")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public Task SendPushTxDialogAsync([FromBody] PushTxDialogModel model)
        {
            return _appNotifications.SendPushTxDialogAsync(model.NotiicationIds.ToArray(), model.Amount, model.AssetId,
                model.AddressFrom, model.AddressTo, model.Message);
        }

        [HttpPost("SendAssetsCreditedNotification")]
        [SwaggerOperation("SendAssetsCreditedNotification")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public Task SendAssetsCreditedNotificationAsync([FromBody] AssetsCreditedModel model)
        {
            return _appNotifications.SendAssetsCreditedNotification(model.NotificationIds.ToArray(), model.Amount,
                model.AssetId, model.Message);
        }

        [HttpPost("SendRawNotification")]
        [SwaggerOperation("SendRawNotification")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public Task SendRawNotificationAsync([FromBody] RawNotificationModel model)
        {
            switch (model.MobileOs)
            {
                case MobileOs.Ios:
                    return _appNotifications.SendRawIosNotification(model.NotificationId, model.Payload);
                case MobileOs.Android:
                    return _appNotifications.SendRawAndroidNotification(model.NotificationId, model.Payload);
                default:
                    throw new Exception("Unsupported OS type");
            }
        }
    }
}
