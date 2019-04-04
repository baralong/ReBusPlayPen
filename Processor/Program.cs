using Autofac;
using Common;
using Rebus.Config;
using Serilog;
using System;

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
                    .Subscriptions(s => s.StoreInSqlServer(Constants.ConnectionString, "Subscriptions", true))
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
