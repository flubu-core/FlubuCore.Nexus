using System;
using System.Collections.Generic;
using System.Text;

namespace FlubuCore.Nexus
{
    using System.Linq;
    using System.Threading.Tasks;

    using FlubuCore.Context;
    using FlubuCore.Infrastructure;
    using FlubuCore.Nexus.Models;
    using FlubuCore.Nexus.Models.Search;
    using FlubuCore.Tasks;

    using Flurl;
    using Flurl.Http;

    public class FindLatestVersionInGroupTask : TaskBase<SearchResponse, FindLatestVersionInGroupTask>
    {
        private readonly string _nexusBaseUrl;

        private readonly string _repository;
        private readonly string _group;
        private bool _versionAsSubGroup;

        private string _returnItemsWithExtension;

        public FindLatestVersionInGroupTask(string nexusBaseUrl, string repository, string group)
        { _nexusBaseUrl = nexusBaseUrl;
            _repository = repository;
            this._group = group;
        }

        public override string TaskName => $"Nexus.{nameof(FindLatestVersionInGroupTask)}";

        /// <summary>
        /// When applied latest version is searched in subgroup. Following format for group must be used in nexus: /{group}/{version} otherwise search wont work.
        /// </summary>
        /// <returns></returns>
        public FindLatestVersionInGroupTask VersionAsSubGroup()
        {
            _versionAsSubGroup = true;
            return this;
        }

        /// <summary>
        /// Returns latest items with only specified extension.
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public FindLatestVersionInGroupTask ReturnItemsWithFileNameExtension(string extension)
        {
            _returnItemsWithExtension = extension;
            return this;
        }

        protected override string Description { get; set; }

        protected override SearchResponse DoExecute(ITaskContextInternal context)
        {
            Task<SearchResponse> task = DoExecuteAsync(context);

            return task.GetAwaiter().GetResult();
        }

        protected override async Task<SearchResponse> DoExecuteAsync(ITaskContextInternal context)
        {

            if (!_versionAsSubGroup)
            {
                throw new NotSupportedException("Currently only version as subgroup is supported. Apply VersionAsSubGroup on task.");
            }

            List<Item> items = new List<Item>();
            SearchResponse searchResult = null;
            do
            {
                if (searchResult == null)
                {
                    searchResult = await _nexusBaseUrl.AppendPathSegment("service/rest/v1/search")
                                       .SetQueryParams(new { repository = _repository })
                                       .GetJsonAsync<SearchResponse>();
                }
                else
                {
                    searchResult = await _nexusBaseUrl.AppendPathSegment("service/rest/v1/search")
                                       .SetQueryParams(new { repository = _repository, continuationToken = searchResult.ContinuationToken })
                                       .GetJsonAsync<SearchResponse>();
                }

                items.AddRange(searchResult.Items);
            }
            while (searchResult.ContinuationToken != null);

            var nisItems = items.Where(x => x.Group.Contains(_group) && x.Name.EndsWith(".zip"));

            Version latestVersion = new Version(0, 0, 0, 1);
            foreach (var item in nisItems)
            {
                var splitedGroup = item.Group.Split('/');
                var version = new Version(splitedGroup[2]);
                if (latestVersion < version)
                {
                    latestVersion = version;
                }
            }

            var latestItems = await _nexusBaseUrl.AppendPathSegment("service/rest/v1/search")
                                  .SetQueryParams(new { repository = _repository, group = $"/{_group}/{latestVersion.ToString()}" })
                                  .GetJsonAsync<SearchResponse>();

            if (!string.IsNullOrEmpty(_returnItemsWithExtension))
            {
                latestItems.Items = latestItems.Items.Where(x => x.Name.EndsWith(_returnItemsWithExtension)).ToList();
            }

            return latestItems;
        }
    }
}
