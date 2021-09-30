using System;
using FlubuCore.Scripting;

namespace FlubuCore.Nexus.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            args = new[] { "Deploy.all.test" };
            var engine = new FlubuEngine();
            var flubuSession = engine.CreateTaskSession(new BuildScriptArguments());
         
              var packages = new FindLatestVersionInGroupTask("http://ctsqlr01.durs.si:8081/", "idis_core", "Idis.Izmenjave")
                    .VersionAsSubGroup()
                    .ReturnItemsWithFileNameExtension("zip")
                    .Execute(flubuSession);
        }
    }
}
