using System;
using System.Collections.Generic;
using System.Text;

namespace FlubuCore.Nexus
{
    using FlubuCore.Infrastructure;

    using Microsoft.Extensions.DependencyInjection;

    public class Nexus
    {
        /// <summary>
        /// Downloads Asset from nexus repository.
        /// </summary>
        /// <param name="nexusBaseUrl">base url to the nexus repository</param>
        /// <param name="repository">Name of the nexus repository.</param>
        /// <param name="group">Nexus repository group.</param>
        /// <returns></returns>
        public FindLatestVersionInGroupTask FindLatestVersionInGroup(string nexusBaseUrl, string repository, string group)
        {
            return new FindLatestVersionInGroupTask(nexusBaseUrl, repository, group);
        }
    }
}
