using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Cqrs;
using Lykke.Service.PushNotifications.Contract.Events;
using Lykke.Service.PushNotifications.Core.Services;

namespace Lykke.Service.PushNotifications.Workflow.Sagas
{
    public class NotificationSaga
    {
        private readonly TimeSpan _retrySeconds = TimeSpan.FromSeconds(30);
        private readonly ILog _log;
        private readonly IAppNotifications _appNotifications;

        public NotificationSaga(ILog log, IAppNotifications appNotifications)
        {
            _log = log;
            _appNotifications = appNotifications;
        }

        public async Task<CommandHandlingResult> Handle(DataNotificationEvent evt, ICommandSender sender)
        {
            try
            {
                await _appNotifications.SendDataNotificationToAllDevicesAsync(
                    evt.NotificationIds.ToArray(),
                    evt.Type,
                    evt.Entity,
                    evt.Id);
            }
            catch (Exception e)
            {
                _log.WriteError(nameof(DataNotificationEvent), evt.ToJson(), e);

                return CommandHandlingResult.Fail(_retrySeconds);
            }

            return CommandHandlingResult.Ok();
        }
    }
}
