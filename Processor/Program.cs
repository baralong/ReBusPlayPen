using Autofac;
using Common;
using Common.Messages;
using Rebus.Bus;
using Rebus.Bus.Advanced;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.Sagas.Exclusive;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Processor
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                                    .WriteTo.Console()
                                    .MinimumLevel.Debug()
                                    .CreateLogger();

            var builder = new ContainerBuilder();
            builder.RegisterRebus((config, context) => config
                    .Logging(l => l.Serilog())
                    .Routing(r => r.TypeBased().MapAssemblyOf<OneWayMessage>("Processor"))
                    .Subscriptions(s => s.StoreInSqlServer(Constants.ConnectionString, "Subscriptions", true))
                    .Sagas(s => 
                    {
                        s.StoreInSqlServer(Constants.ConnectionString, "Saga", "SagaIndex");
                        s.EnforceExclusiveAccess();
                    })
                    .Transport(t => t.UseSqlServer(Constants.ConnectionString, "Processor"))
                    .Options(o =>
                    {
                        o.SetNumberOfWorkers(2);
                        o.SetMaxParallelism(30);
                    }));
            builder.RegisterHandlersFromAssemblyOf<Program>();

            using (var container = builder.Build())
            {
                Console.WriteLine("Press <enter> to exit;");
                Console.ReadLine();
            }
        }
    }
}
