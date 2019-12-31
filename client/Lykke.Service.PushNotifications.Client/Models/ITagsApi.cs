using System.Threading.Tasks;
using JetBrains.Annotations;
using Refit;

namespace Lykke.Service.PushNotifications.Client.Models
{
    [PublicAPI]
    public interface ITagsApi
    {
        [Get("/api/Tags")]
        Task<TagsResponse> GetTagsAsync();
        [Get("/api/Tags/{clientId}")]
        Task<ClientTagsResponse> GetClientTagsAsync(string clientId);
        [Post("/api/Tags/{clientId}/{tag}")]
        Task AddClientTagAsync(string clientId, string tag);
        [Post("/api/Tags")]
        Task CreateTagAsync([Body] string tag);
        [Delete("/api/Tags")]
        Task DeleteTagAsync([Body] string tag);
    }
}
