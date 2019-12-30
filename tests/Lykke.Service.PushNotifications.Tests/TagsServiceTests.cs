using System.Linq;
using System.Threading.Tasks;
using AzureStorage.Tables;
using AzureStorage.Tables.Templates.Index;
using Lykke.Service.PushNotifications.AzureRepositories;
using Lykke.Service.PushNotifications.Core.Services;
using Lykke.Service.PushNotifications.Services;
using Xunit;

namespace Lykke.Service.PushNotifications.Tests
{
    public class TagsServiceTests
    {
        private readonly ITagsService _service;

        public TagsServiceTests()
        {
            _service = new TagsService(
                new TagsRepository(new NoSqlTableInMemory<TagEntity>()),
                new ClientTagsRepository(new NoSqlTableInMemory<ClientTagEntity>(), new NoSqlTableInMemory<AzureIndex>())
            );
        }

        [Fact]
        public async Task Tags_Are_Created()
        {
            await _service.CreateTagAsync("tag1");
            await _service.CreateTagAsync("tag2");

            var tags = await _service.GetTagsAsync();

            Assert.Equal(2, tags.Length);
            Assert.Contains(tags, x => x.Tag == "tag1");
            Assert.Contains(tags, x => x.Tag == "tag2");
        }

        [Fact]
        public async Task ClientTags_Are_Created()
        {
            const string clientId = "1";

            await _service.CreateTagAsync("tag1");
            await _service.CreateTagAsync("tag2");

            await _service.AddClientTagAsync(clientId, "tag2");

            var clientTags = await _service.GetClientTagsAsync(clientId);
            var tags = await _service.GetTagsAsync();

            Assert.Single(clientTags);
            Assert.Contains(clientTags, x => x == "tag2");

            Assert.Equal(0, tags.First(x => x.Tag == "tag1").Count);
            Assert.Equal(1, tags.First(x => x.Tag == "tag2").Count);
        }

        [Fact]
        public async Task NonexistingTag_Not_Added_To_ClientTags()
        {
            const string clientId = "1";

            await _service.CreateTagAsync("tag1");
            await _service.CreateTagAsync("tag2");

            await _service.AddClientTagAsync(clientId, "zzz");

            var clientTags = await _service.GetClientTagsAsync(clientId);

            Assert.Empty(clientTags);
        }

        [Fact]
        public async Task Tags_Are_Deleted()
        {
            await _service.CreateTagAsync("tag1");
            await _service.CreateTagAsync("tag2");
            await _service.DeleteTagAsync("tag2");

            var tags = await _service.GetTagsAsync();

            Assert.Single(tags);
            Assert.Contains(tags, x => x.Tag == "tag1");
        }

        [Fact]
        public async Task ClientTags_Are_Deleted()
        {
            const string clientId = "1";

            await _service.CreateTagAsync("tag1");
            await _service.CreateTagAsync("tag2");
            await _service.AddClientTagAsync(clientId, "tag1");
            await _service.AddClientTagAsync(clientId, "tag2");

            await _service.DeleteTagAsync("tag1");

            var clientTags = await _service.GetClientTagsAsync(clientId);

            Assert.Single(clientTags);
            Assert.Contains(clientTags, x => x == "tag2");
        }
    }
}
