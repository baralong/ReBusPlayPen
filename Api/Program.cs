using Autofac;
using Common;
using Common.Messages;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Serilog;
using System;
using System.Linq;
using System.Reflection;

namespace Api
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
                    .Transport(t => t.UseSqlServer(Constants.ConnectionString, "Api"))
                    .Routing(r => r.TypeBased().MapAssemblyOf<OneWayMessage>("Processor"))
                    .Subscriptions(s => s.StoreInSqlServer(Constants.ConnectionString, "Subscriptions", true))
                    .Options(o =>
                    {
                        o.SetNumberOfWorkers(2);
                        o.SetMaxParallelism(30);
                    }));

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                        .Where(t => t.IsAssignableTo<IUserCommand>())
                        .AsImplementedInterfaces();

            using (var container = builder.Build())
            {
                var commands = container.Resolve<IUserCommand[]>()
                    .OrderBy(uc => uc.Key)
                    .ToDictionary(uc => uc.Key);
                var prompt = string.Join("\n", commands
                        .Select(pair => $"{pair.Key}) {pair.Value.Description}"))
                        + "\nq) to quit";

                while(true)
                {
                    Console.WriteLine(prompt);
                    var key = char.ToLower(Console.ReadKey().KeyChar);
                    if (key == 'q') return;
                    Console.WriteLine();
                    if (commands.TryGetValue(key, out IUserCommand cmd))
                    {
                        cmd.ExecuteAsync().Wait();
                    }
                    else
                    {
                        Console.WriteLine($"Invalid choice '{key}'");
                    }
                }
            }
        }
    }
}
