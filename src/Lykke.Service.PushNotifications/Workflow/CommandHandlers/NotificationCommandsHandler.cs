using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Cqrs;
using Lykke.Service.PushNotifications.Contract.Commands;
using Lykke.Service.PushNotifications.Contract.Enums;
using Lykke.Service.PushNotifications.Core.Services;

namespace Lykke.Service.PushNotifications.Workflow.CommandHandlers
{
    public class NotificationCommandsHandler
    {
        private readonly TimeSpan _retrySeconds = TimeSpan.FromSeconds(30);
        private readonly ILog _log;
        private readonly IAppNotifications _appNotifications;

        public NotificationCommandsHandler(ILog log,
            IAppNotifications appNotifications)
        {
            _log = log.CreateComponentScope(nameof(NotificationCommandsHandler));
            _appNotifications = appNotifications;
        }

        public async Task<CommandHandlingResult> Handle(AssetsCreditedCommand command)
        {
            try
            {
                await _appNotifications.SendAssetsCreditedNotification(command.NotificationIds.ToArray(), command.Amount,
                    command.AssetId, command.Message);
            }
            catch (Exception e)
            {
                _log.WriteError(nameof(AssetsCreditedCommand), command.ToJson(), e, DateTime.UtcNow);

                return CommandHandlingResult.Fail(_retrySeconds);
            }

            return CommandHandlingResult.Ok();
        }

        public async Task<CommandHandlingResult> Handle(DataNotificationCommand command)
        {
            try
            {
                await _appNotifications.SendDataNotificationToAllDevicesAsync(command.NotificationIds.ToArray(),
                    command.Type, command.Entity, command.Id);
            }
            catch (Exception e)
            {
                _log.WriteError(nameof(DataNotificationCommand), command.ToJson(), e, DateTime.UtcNow);

                return CommandHandlingResult.Fail(_retrySeconds);
            }

            return CommandHandlingResult.Ok();

        }

        public async Task<CommandHandlingResult> Handle(PushTxDialogCommand command)
        {
            try
            {
                await _appNotifications.SendPushTxDialogAsync(command.NotiicationIds.ToArray(), command.Amount, command.AssetId,
                    command.AddressFrom, command.AddressTo, command.Message);
            }
            catch (Exception e)
            {
                _log.WriteError(nameof(PushTxDialogCommand), command.ToJson(), e, DateTime.UtcNow);

                return CommandHandlingResult.Fail(_retrySeconds);
            }

            return CommandHandlingResult.Ok();
        }

        public async Task<CommandHandlingResult> Handle(RawNotificationCommand command)
        {
            try
            {
                switch (command.MobileOs)
                {
                    case MobileOs.Ios:
                        await _appNotifications.SendRawIosNotification(command.NotificationId, command.Payload);
                        break;
                    case MobileOs.Android:
                        await _appNotifications.SendRawAndroidNotification(command.NotificationId, command.Payload);
                        break;
                    default:
                        _log.WriteError(nameof(RawNotificationCommand), command.ToJson(), new Exception("Unsupported Mobile Type"), DateTime.UtcNow);

                        return CommandHandlingResult.Ok();
                }
            }
            catch (Exception e)
            {
                _log.WriteError(nameof(RawNotificationCommand), command.ToJson(), e, DateTime.UtcNow);

                return CommandHandlingResult.Fail(_retrySeconds);
            }

            return CommandHandlingResult.Ok();
        }

        public async Task<CommandHandlingResult> Handle(TextNotificationCommand command)
        {
            try
            {
                await _appNotifications.SendTextNotificationAsync(command.NotificationIds.ToArray(), command.Type,
                    command.Message);
            }
            catch (Exception e)
            {
                _log.WriteError(nameof(TextNotificationCommand), command.ToJson(), e, DateTime.UtcNow);

                return CommandHandlingResult.Fail(_retrySeconds);
            }

            return CommandHandlingResult.Ok();
        }

        public async Task<CommandHandlingResult> Handle(LimitOrderNotificationCommand command)
        {
            try
            {
                await _appNotifications.SendLimitOrderNotification(
                    command.NotificationIds.ToArray(),
                    command.Message,
                    command.OrderType,
                    command.OrderStatus);
            }
            catch (Exception e)
            {
                _log.WriteError(nameof(LimitOrderNotificationCommand), command.ToJson(), e, DateTime.UtcNow);

                return CommandHandlingResult.Fail(_retrySeconds);
            }

            return CommandHandlingResult.Ok();
        }
        
        public async Task<CommandHandlingResult> Handle(MtOrderChangedNotificationCommand command)
        {
            try
            {
                await _appNotifications.SendMtOrderChangedNotification(
                    command.NotificationIds == null
                        ? Array.Empty<string>()
                        : command.NotificationIds.ToArray(),
                    command.Type,
                    command.Message,
                    command.OrderId);
            }
            catch (Exception e)
            {
                _log.WriteError(nameof(MtOrderChangedNotificationCommand), command.ToJson(), e, DateTime.UtcNow);

                return CommandHandlingResult.Fail(_retrySeconds);
            }

            return CommandHandlingResult.Ok();
        }
    }
}
