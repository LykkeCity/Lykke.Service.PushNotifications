using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.PushNotifications.Client.Models;
using Lykke.Service.PushNotifications.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Lykke.Service.PushNotifications.Controllers
{
    [Route("api/Tags")]
    public class TagsController : Controller, ITagsApi
    {
        private readonly ITagsService _tagsService;
        private readonly IMapper _mapper;

        public TagsController(
            ITagsService tagsService,
            IMapper mapper
            )
        {
            _tagsService = tagsService;
            _mapper = mapper;
        }

        [HttpGet]
        [SwaggerOperation("GetAllTags")]
        [ProducesResponseType(typeof(TagsResponse), (int)HttpStatusCode.OK)]
        public async Task<TagsResponse> GetTagsAsync()
        {
            var tags = await _tagsService.GetTagsAsync();

            return new TagsResponse { Tags = _mapper.Map<TagItem[]>(tags) };
        }

        [HttpGet("{clientId}")]
        [SwaggerOperation("GetClientTags")]
        [ProducesResponseType(typeof(ClientTagsResponse), (int)HttpStatusCode.OK)]
        public async Task<ClientTagsResponse> GetClientTagsAsync(string clientId)
        {
            string[] tags = await _tagsService.GetClientTagsAsync(clientId);

            return new ClientTagsResponse { Tags = tags };
        }

        [HttpPost("{clientId}/{tag}")]
        [SwaggerOperation("AddClientTag")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task AddClientTagAsync(string clientId, string tag)
        {
            return _tagsService.AddClientTagAsync(clientId, tag);
        }

        [HttpPost]
        [SwaggerOperation("CreateTag")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task CreateTagAsync([FromBody]string tag)
        {
            return _tagsService.CreateTagAsync(tag);
        }

        [HttpDelete]
        [SwaggerOperation("DeleteTag")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task DeleteTagAsync([FromBody]string tag)
        {
            return _tagsService.DeleteTagAsync(tag);
        }
    }
}
