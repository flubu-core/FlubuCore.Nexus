using System;

namespace FlubuCore.Nexus
{       
    using FlubuCore.Context.FluentInterface.Interfaces;

    public static class NexusExtensions
    {
        public static Nexus Nexus(this ITaskFluentInterface flubu)
        {
            return new Nexus();
        }
    }
}