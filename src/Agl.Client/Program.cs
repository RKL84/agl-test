using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Serilog;
using Agl.Core.Configuration;
using Microsoft.Extensions.Options;
using Agl.Core.Infrastructure.Services;
using System;
using System.Net.Http;
using Polly;
using Polly.Extensions.Http;

namespace Agl.Client
{
    class Program
    {
        public static IConfiguration Configuration { get; set; }

        /// <summary>
        /// The entry point for the program
        /// </summary>
        /// <param name="args">The arguments.</param>
        static void Main(string[] args)
        {
            Configuration = GetConfiguration();
            Log.Logger = CreateSerilogLogger(Configuration);
            var services = new ServiceCollection();
            ConfigureServices(services);

            var serviceProvider = services.BuildServiceProvider();
            var writer = serviceProvider.GetService<PeopleInfoConsoleWriter>();
            writer.Run().Wait();
            Console.Read();
        }

        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services</param>
        static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);
            services.AddOptions();

            services.Configure<AppSettings>(options => 
            Configuration.GetSection("AppSettings").Bind(options));
            services.AddSingleton(sp => sp.GetService<IOptions<AppSettings>>().Value);

            services.AddHttpClient<IPeopleService, PeopleService>()
                 .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                 .AddPolicyHandler(GetRetryPolicy());

            services.AddTransient<PeopleInfoConsoleWriter>();
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            //policy is configured to try six times with 
            //an exponential retry, starting at two seconds
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(6, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        private static ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        private static IConfiguration GetConfiguration()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            return builder.Build();
        }
    }
}
