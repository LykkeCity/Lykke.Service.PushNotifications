using AutoMapper;
using Lykke.Service.PushNotifications.Contract;
using Lykke.Service.PushNotifications.Core.Domain;

namespace Lykke.Service.PushNotifications.Profiles
{
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            CreateMap<IInstallation, DeviceInstallation>(MemberList.Source);
        }
    }
}
