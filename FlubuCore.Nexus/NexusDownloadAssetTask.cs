using System;
using System.Collections.Generic;
using System.Text;

namespace FlubuCore.Nexus
{
    using System.Threading.Tasks;

    using FlubuCore.Context;
    using FlubuCore.Infrastructure;
    using FlubuCore.Nexus.Models;
    using FlubuCore.Tasks;

    using Flurl;
    using Flurl.Http;

    public class NexusDownloadAssetTask : TaskBase<int, NexusDownloadAssetTask>
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly string _nexusBaseUrl;

        private readonly string _repository;

        private readonly string _group;

        private readonly string _name;

        public NexusDownloadAssetTask(IHttpClientFactory httpClientFactory, string nexusBaseUrl, string repository, string group, string name)
        {
            _httpClientFactory = httpClientFactory;
            _nexusBaseUrl = nexusBaseUrl;
            _repository = repository;
            _group = @group;
            _name = name;
        }

        protected override string Description { get; set; }

        protected override int DoExecute(ITaskContextInternal context)
        {
            Task<int> task = DoExecuteAsync(context);

            return task.GetAwaiter().GetResult();
        }

        protected override async Task<int> DoExecuteAsync(ITaskContextInternal context)
        {
            var searchResult = await _nexusBaseUrl
                                   .AppendPathSegment("service/rest/v1/search")
                                   .SetQueryParams(new { repository = _repository })
                                   .GetJsonAsync<SearchResponse>();

            Item latestItem = null;
            for (var index = 0; index < searchResult.Items.Count - 1; index++)
            {
                if (searchResult.Items[index].Version < searchResult.Items[index + 1].Version)
                {
                    if (searchResult.Items[index].Version == null)
                    {
                        continue;
                    }

                    latestItem = searchResult.Items[index];
                }
            }

            if (latestItem != null)
            {
               var httpClient = _httpClientFactory.Create(_nexusBaseUrl);
               await httpClient.DownloadFileAsync(latestItem.Assets[0].DownloadUrl, latestItem.Assets[0].Path);
            }

            return 0;
        }
    }
}
