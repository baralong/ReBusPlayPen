using Autofac;
using Rebus.Config;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processor
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterRebus((config, context) =>
                config
                    .Logging(l => l.Serilog(new LoggerConfiguration().WriteTo.Console()))
                    .Transport(t => t.UseSqlServer("server=.; initial catalog=rebus; integrated security=true", "Processor"))
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
