using Lykke.Service.PushNotifications.Core.Domain;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.PushNotifications.AzureRepositories
{
    public class TagEntity : TableEntity, ITag
    {
        public string Tag => RowKey;
        public int Count { get; set; }

        public static string GeneratePk() => "Tag";

        public static TagEntity Create(string tag)
        {
            return new TagEntity
            {
                PartitionKey = GeneratePk(),
                RowKey = tag
            };
        }
    }
}
