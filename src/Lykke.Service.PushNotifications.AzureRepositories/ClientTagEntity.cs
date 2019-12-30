using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.PushNotifications.AzureRepositories
{
    public class ClientTagEntity : TableEntity
    {
        public string ClientId => PartitionKey;
        public string Tag => RowKey;

        public static ClientTagEntity Create(string clientId, string tag)
        {
            return new ClientTagEntity
            {
                PartitionKey = clientId,
                RowKey = tag
            };
        }
    }
}
