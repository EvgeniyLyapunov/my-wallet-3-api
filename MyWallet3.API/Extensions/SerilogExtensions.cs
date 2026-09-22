using Serilog;

namespace MyWallet3.API.Extensions
{
    public static class SerilogExtensions
    {
        public static void ConfigureSerilog(this WebApplicationBuilder builder)
        {
            var exePath = AppContext.BaseDirectory;
            var logFilePath = Path.Combine(exePath, "logs", "log.txt");
            var logTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}";
            builder.Host.UseSerilog((context, loggerConfig) =>
            {
                loggerConfig
                    .ReadFrom.Configuration(context.Configuration)
                    .Enrich.FromLogContext()
                    .WriteTo.Console(outputTemplate: logTemplate)
                    .WriteTo.File(
                        path: logFilePath, 
                        rollingInterval: RollingInterval.Day, 
                        outputTemplate: logTemplate);
            });
        }
    }
}
