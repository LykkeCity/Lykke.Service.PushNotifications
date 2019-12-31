using System;

namespace Lykke.Service.PushNotifications.Client.Models
{
    public class TagsResponse
    {
        public TagItem[] Tags { get; set; } = Array.Empty<TagItem>();
    }
}
