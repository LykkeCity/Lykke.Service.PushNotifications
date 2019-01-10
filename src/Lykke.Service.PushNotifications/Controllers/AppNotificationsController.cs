using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Service.PushNotifications.Contract.Enums;
using Lykke.Service.PushNotifications.Core.Filters;
using Lykke.Service.PushNotifications.Core.Services;
using Lykke.Service.PushNotifications.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.PushNotifications.Controllers
{
    [Route("api/[controller]")]
    [ValidateModel]
    public class AppNotificationsController:  Controller
    {
        private readonly IAppNotifications _appNotifications;
        private readonly ILog _log;

        public AppNotificationsController(IAppNotifications appNotifications, ILog log)
        {
            _appNotifications = appNotifications;
            _log = log;
        }

        [HttpPost("SendDataNotificationToAllDevices")]
        [SwaggerOperation("SendDataNotificationToAllDevices")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SendDataNotificationToAllDevicesAsync([FromBody] DataNotificationModel model)
        {
            try
            {
                await _appNotifications.SendDataNotificationToAllDevicesAsync(model.NotificationIds.ToArray(), model.Type, model.Entity, model.Id);
            }
            catch (Exception e)
            {
                _log.WriteError($"{nameof(AppNotificationsController)}:{nameof(SendDataNotificationToAllDevicesAsync)}", model.ToJson(), e);
                return BadRequest(ErrorResponse.Create(e.Message));
            }

            return Ok();
        }

        [HttpPost("SendTextNotification")]
        [SwaggerOperation("SendTextNotification")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SendTextNotificationAsync([FromBody] TextNotificationModel model)
        {
            try
            {
                await _appNotifications.SendTextNotificationAsync(model.NotificationIds.ToArray(), model.Type,
                    model.Message);
            }
            catch (Exception e)
            {
                _log.WriteError($"{nameof(AppNotificationsController)}:{nameof(SendTextNotificationAsync)}", model.ToJson(), e);
                return BadRequest(ErrorResponse.Create(e.Message));
            }

            return Ok();
        }

        [HttpPost("SendPushTxDialog")]
        [SwaggerOperation("SendPushTxDialog")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SendPushTxDialogAsync([FromBody] PushTxDialogModel model)
        {
            try
            {
                await _appNotifications.SendPushTxDialogAsync(model.NotiicationIds.ToArray(), model.Amount, model.AssetId,
                    model.AddressFrom, model.AddressTo, model.Message);
            }
            catch (Exception e)
            {
                _log.WriteErrorAsync($"{nameof(AppNotificationsController)}:{nameof(SendPushTxDialogAsync)}", model.ToJson(), e);
                return BadRequest(ErrorResponse.Create(e.Message));
            }

            return Ok();
        }

        [HttpPost("SendAssetsCreditedNotification")]
        [SwaggerOperation("SendAssetsCreditedNotification")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SendAssetsCreditedNotificationAsync([FromBody] AssetsCreditedModel model)
        {
            try
            {
                await _appNotifications.SendAssetsCreditedNotification(model.NotificationIds.ToArray(), model.Amount,
                    model.AssetId, model.Message);
            }
            catch (Exception e)
            {
                _log.WriteErrorAsync($"{nameof(AppNotificationsController)}:{nameof(SendAssetsCreditedNotificationAsync)}", model.ToJson(), e);
                return BadRequest(ErrorResponse.Create(e.Message));
            }

            return Ok();
        }

        [HttpPost("SendRawNotification")]
        [SwaggerOperation("SendRawNotification")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SendRawNotificationAsync([FromBody] RawNotificationModel model)
        {
            try
            {
                switch (model.MobileOs)
                {
                    case MobileOs.Ios:
                        await _appNotifications.SendRawIosNotification(model.NotificationId, model.Payload);
                        break;
                    case MobileOs.Android:
                        await _appNotifications.SendRawAndroidNotification(model.NotificationId, model.Payload);
                        break;
                    default:
                        throw new Exception("Unsupported OS type");
                }
            }
            catch (Exception e)
            {
                _log.WriteErrorAsync($"{nameof(AppNotificationsController)}:{nameof(SendRawNotificationAsync)}", model.ToJson(), e);
                return BadRequest(ErrorResponse.Create(e.Message));
            }

            return Ok();
        }
    }
}
