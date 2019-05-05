using Autofac;
using Common;
using Common.IO;
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
                        o.EnableSynchronousRequestReply();
                    }));

            builder.RegisterHandler<LogSagaState>();

            builder.RegisterType<MyConsole>().AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                        .Where(t => t.IsAssignableTo<IUserCommand>())
                        .AsImplementedInterfaces();

            using (var container = builder.Build())
            {
                var commands = container.Resolve<IUserCommand[]>()
                    .Select((command, i) => new {command, key = (char)(i + 'a') })
                    .ToDictionary(tmp => tmp.key, tmp => tmp.command);

                var prompt = "\nSelect a command\n" + 
                    string.Join("\n", commands
                        .Select(pair => $"{pair.Key}) {pair.Value.Description}"))
                        + "\nq) to quit";

                while(true)
                {
                    Console.WriteLine(prompt);
                    Console.Write("> ");

                    var key = char.ToLower(Console.ReadKey().KeyChar);
                    Console.WriteLine();

                    if (key == 'q') return;
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
