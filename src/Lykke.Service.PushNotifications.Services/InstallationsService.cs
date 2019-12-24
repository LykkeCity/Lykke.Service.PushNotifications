using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Service.PushNotifications.Contract.Enums;
using Lykke.Service.PushNotifications.Core.Domain;
using Lykke.Service.PushNotifications.Core.Services;
using Microsoft.Azure.NotificationHubs;

namespace Lykke.Service.PushNotifications.Services
{
    public class InstallationsService : IInstallationsService
    {
        private readonly NotificationHubClient _notificationHubClient;
        private readonly IInstallationsRepository _installationsRepository;

        public InstallationsService(
            NotificationHubClient notificationHubClient,
            IInstallationsRepository installationsRepository
            )
        {
            _notificationHubClient = notificationHubClient;
            _installationsRepository = installationsRepository;
        }

        public async Task<string> RegisterAsync(string clientId, string installationId, string notificationId, MobileOs platform, string pushChannel)
        {
            var installationItem = !string.IsNullOrEmpty(installationId)
                ? await _installationsRepository.GetAsync(clientId, installationId)
                : (await _installationsRepository.GetByClientIdAsync(clientId))
                    .FirstOrDefault(x => x.PushChannel.Equals(pushChannel, StringComparison.InvariantCultureIgnoreCase));

            string[] tags = new HashSet<string>(installationItem?.Tags ?? Array.Empty<string>())
            {
                notificationId,
                platform.ToString()
            }.ToArray();

            Installation installation = new Installation
            {
                InstallationId = installationId ?? installationItem?.InstallationId ?? Guid.NewGuid().ToString(),
                PushChannel = GetPushChannel(platform, pushChannel),
                Tags = tags,
                Platform = platform == MobileOs.Ios
                    ? NotificationPlatform.Apns
                    : NotificationPlatform.Fcm
            };

            await _notificationHubClient.CreateOrUpdateInstallationAsync(installation);
            await _installationsRepository.AddOrUpdateAsync(new InstallationItem
            {
                ClientId = clientId,
                NotificationId = notificationId,
                InstallationId = installation.InstallationId,
                PushChannel = installation.PushChannel,
                Platform = platform,
                Tags = tags
            });

            return installation.InstallationId;
        }

        private string GetPushChannel(MobileOs platform, string pushChannel)
        {
            switch (platform)
            {
                case MobileOs.Ios:
                    return pushChannel.Trim('<', '>').Replace(" ", "").ToUpperInvariant();
                case MobileOs.Android:
                    return pushChannel;
                default:
                    throw new ArgumentOutOfRangeException(nameof(platform), platform, null);
            }
        }
    }
}
