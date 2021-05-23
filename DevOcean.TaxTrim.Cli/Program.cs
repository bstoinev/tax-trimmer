using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DevOcean.TaxTrim.Cli
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var log = new NLogLoggingFacility<Program>();
            
            IHost host;
            try
            {
                var builder = Host.CreateDefaultBuilder()
                    // Suppress internal logging as we use our own abstraction. See ILoggingFacility 
                    .ConfigureLogging(l => l.ClearProviders())
                    .ConfigureServices((ctx, services) =>
                    {
                        var cfg = ctx.Configuration;

                        services.Configure<TaxSettings>(cfg.GetSection("TaxSettings"));
                        
                        services.AddSingleton(typeof(ILoggingFacility<>), typeof(NLogLoggingFacility<>));

                        services.AddTransient<TaxCalculator>();

                        services.AddHostedService<TaxCalculatorService>();
                    });

                host = builder.Build();

                var ver = typeof(Program).Assembly.GetName().Version;

                log.Info($"Tax Trimmer {ver} initialized.");
                log.Info("Press Ctrl+C to exit.");
            }
            catch (Exception ex)
            {
                log.Warning("Startup error has occured. Aborting... ");
                log.Error(ex);
                throw;
            }

            await host.RunAsync();
        }
    }
}
