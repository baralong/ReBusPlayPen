using Autofac;
using Common;
using Rebus.Config;
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
            var builder = new ContainerBuilder();
            builder.RegisterRebus((config, context) =>
                config
                    .Logging(l => l.Serilog(new LoggerConfiguration().WriteTo.Console()))
                    .Transport(t => t.UseSqlServer(Constants.ConnectionString, "Api"))
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
                        cmd.ExecuteAsync();
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
