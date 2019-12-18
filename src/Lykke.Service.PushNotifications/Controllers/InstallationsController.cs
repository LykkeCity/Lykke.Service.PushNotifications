using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Common.Log;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Common.ApiLibrary.Exceptions;
using Lykke.Common.Log;
using Lykke.Service.PushNotifications.Client;
using Lykke.Service.PushNotifications.Client.Models;
using Lykke.Service.PushNotifications.Contract;
using Lykke.Service.PushNotifications.Contract.Enums;
using Lykke.Service.PushNotifications.Core.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.NotificationHubs;
using Swashbuckle.AspNetCore.Annotations;

namespace Lykke.Service.PushNotifications.Controllers
{
    [Route("api/Installations")]
    public class InstallationsController : Controller, IInstallationsApi
    {
        private readonly NotificationHubClient _notificationHubClient;
        private readonly IInstallationsRepository _installationsRepository;
        private readonly ILog _log;

        public InstallationsController(
            NotificationHubClient notificationHubClient,
            IInstallationsRepository installationsRepository,
            ILogFactory logFactory
        )
        {
            _notificationHubClient = notificationHubClient;
            _installationsRepository = installationsRepository;
            _log = logFactory.CreateLog(this);
        }

        [HttpPost]
        [SwaggerOperation("Register")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task RegisterAsync([FromBody] InstallationModel model)
        {
            string[] tags = new HashSet<string>(model.Tags)
            {
                model.NotificationId,
                model.Platform.ToString()
            }.ToArray();

            Installation installation = new Installation
            {
                InstallationId = Guid.NewGuid().ToString(),
                PushChannel = GetPushChannel(model.Platform, model.PushChannel),
                Tags = tags,
                Platform = model.Platform == MobileOs.Ios
                    ? NotificationPlatform.Apns
                    : NotificationPlatform.Fcm
            };

            try
            {
                await _notificationHubClient.CreateOrUpdateInstallationAsync(installation);
                await _installationsRepository.AddOrUpdateAsync(new InstallationItem
                {
                    ClientId = model.ClientId,
                    NotificationId = model.NotificationId,
                    InstallationId = installation.InstallationId,
                    PushChannel = installation.PushChannel,
                    Platform = model.Platform,
                    Tags = tags
                });
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                throw new ValidationApiException(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [SwaggerOperation("RemoveInstallation")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task RemoveAsync([FromBody] InstallationRemoveModel model)
        {
            await _notificationHubClient.DeleteInstallationAsync(model.InstallationId);
            await _installationsRepository.DeleteAsync(model.ClientId, model.InstallationId);
        }

        [HttpGet("{clientId}")]
        [SwaggerOperation("GetInstallations")]
        [ProducesResponseType(typeof(IReadOnlyList<DeviceInstallation>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IReadOnlyList<DeviceInstallation>> GetByClientIdAsync(string clientId)
        {
            var installations = await _installationsRepository.GetByClientIdAsync(clientId);
            return Mapper.Map<IReadOnlyList<DeviceInstallation>>(installations);
        }

        [HttpPost("tags")]
        [SwaggerOperation("AddTags")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task AddTagsAsync([FromBody] TagsUpdateModel tagsModel)
        {
            var installation = await _notificationHubClient.GetInstallationAsync(tagsModel.InstallationId);

            if (installation == null)
                throw new ValidationApiException(HttpStatusCode.NotFound, "Installation not found");

            var tags = new HashSet<string>(installation.Tags);

            foreach (var tag in tagsModel.Tags)
            {
                tags.Add(tag);
            }

            installation.Tags = tags.ToArray();

            await _notificationHubClient.CreateOrUpdateInstallationAsync(installation);
            await _installationsRepository.AddOrUpdateAsync(new InstallationItem
            {
                ClientId = tagsModel.ClientId,
                NotificationId = tagsModel.NotificationId,
                InstallationId = tagsModel.InstallationId,
                PushChannel = installation.PushChannel,
                Platform = installation.Platform == NotificationPlatform.Fcm
                    ? MobileOs.Android
                    : MobileOs.Ios,
                Tags = tags.ToArray()
            });
        }

        [HttpDelete("tags")]
        [SwaggerOperation("RemoveTags")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task RemoveTagsAsync([FromBody] TagsUpdateModel tagsModel)
        {
            var installation = await _notificationHubClient.GetInstallationAsync(tagsModel.InstallationId);

            if (installation == null)
                throw new ValidationApiException(HttpStatusCode.NotFound, "Installation not found");

            var tags = new HashSet<string>(installation.Tags);

            foreach (var tag in tagsModel.Tags)
            {
                tags.Remove(tag);
            }

            var platform = installation.Platform == NotificationPlatform.Fcm
                ? MobileOs.Android
                : MobileOs.Ios;

            tags.Add(tagsModel.NotificationId);
            tags.Add(platform.ToString());

            installation.Tags = tags.ToArray();

            await _notificationHubClient.CreateOrUpdateInstallationAsync(installation);
            await _installationsRepository.AddOrUpdateAsync(new InstallationItem
            {
                ClientId = tagsModel.ClientId,
                NotificationId = tagsModel.NotificationId,
                InstallationId = tagsModel.InstallationId,
                PushChannel = installation.PushChannel,
                Platform = platform,
                Tags = tags.ToArray()
            });
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
