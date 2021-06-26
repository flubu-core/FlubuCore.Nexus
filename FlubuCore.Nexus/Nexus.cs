using System;
using System.Collections.Generic;
using System.Text;

namespace FlubuCore.Nexus
{
    using FlubuCore.Infrastructure;

    public class Nexus
    {
        /// <summary>
        /// Downloads Asset from nexus repository.
        /// </summary>
        /// <param name="repository">Name of the nexus repository.</param>
        /// <param name="group">Nexus repository group.</param>
        /// <param name="name">Name of the asset to be downloaded.</param>
        /// <returns></returns>
        public NexusDownloadAssetTask DownloadAsset(string nexusBaseUrl, string repository, string group, string name)
        {
            return new NexusDownloadAssetTask(new HttpClientFactory(), nexusBaseUrl, repository, group, name);
        }
    }
}
