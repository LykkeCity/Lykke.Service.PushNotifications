using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Cqrs;
using Lykke.Service.PushNotifications.Contract.Commands;
using Lykke.Service.PushNotifications.Contract.Enums;
using Lykke.Service.PushNotifications.Core.Services;

namespace Lykke.Service.PushNotifications.Workflow.CommandHandlers
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class NotificationCommandsHandler
    {
        private readonly TimeSpan _retrySeconds = TimeSpan.FromSeconds(30);
        private readonly ILog _log;
        private readonly IAppNotifications _appNotifications;

        public NotificationCommandsHandler(
            ILogFactory logFactory,
            IAppNotifications appNotifications)
        {
            _log = logFactory.CreateLog(this);
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
                _log.Error(e, context: command);

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
                _log.Error(e, context: command);

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
                _log.Error(e, context: command);

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
                        _log.Error(new Exception("Unsupported Mobile Type"), context: command.ToJson());

                        return CommandHandlingResult.Ok();
                }
            }
            catch (Exception e)
            {
                _log.Error(e, context: command);

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
                _log.Error(e, context: command);

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
                _log.Error(e, context: command);

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
                _log.Error(e, context: command);

                return CommandHandlingResult.Fail(_retrySeconds);
            }

            return CommandHandlingResult.Ok();
        }

        public async Task<CommandHandlingResult> Handle(WakeUpNotificationCommand command)
        {
            try
            {
                await _appNotifications.SendWakeupNotificationAsync(command.Tag, command.Message);
            }
            catch (Exception e)
            {
                _log.Error(e, context: command);

                return CommandHandlingResult.Fail(_retrySeconds);
            }

            return CommandHandlingResult.Ok();
        }
    }
}
