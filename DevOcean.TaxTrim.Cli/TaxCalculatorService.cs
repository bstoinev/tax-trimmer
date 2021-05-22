using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DevOcean.TaxTrim.Cli
{
    class TaxCalculatorService : BackgroundService
    {
        private readonly ILoggingFacility<TaxCalculatorService> Log;
        private readonly TaxCalculator Calculator;

        public TaxCalculatorService(TaxCalculator calculator, ILoggingFacility<TaxCalculatorService> log)
        {
            Calculator = calculator;
            Log = log;
        }

        protected override async Task ExecuteAsync(CancellationToken stopper)
        {
            _ = Task.Run(() => ReadConsole(stopper), stopper);

            while (!stopper.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(Timeout.Infinite, stopper);
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine();
                    Log.Info("Tax Trimmer is shutting down.");
                }
                catch (Exception ex)
                {
                    Log.Error($"Unhandled exception: {ex}");
                }
            }
        }
        
        protected async Task ReadConsole(CancellationToken stopper)
        {
            while (!stopper.IsCancellationRequested)
            {
                Console.Write("Gross amount> ");
                var input = Console.ReadLine();

                if (!string.IsNullOrEmpty(input))
                {
                    if (decimal.TryParse(input, out var gross))
                    {
                        var net = Calculator.Trim(gross);

                        Log.Info(net.ToString("N"));
                    }
                    else
                    {
                        Log.Error($"Please enter a positive, decimal number!");
                        Log.Info($"* Note that the maximum acceptable value is {decimal.MaxValue:N}");
                    }
                }

                // Apparently the infrastructure needs some time to process the Ctrl+C properly 
                // A slight delay is introduced so the loop doesn't start prematurely and print unnecessary text on the console.
                await Task.Delay(TimeSpan.FromMilliseconds(100), stopper);
            }
        }
    }
}
