using Microsoft.OpenApi.Models;
using MyWallet3.API.Extensions;
using Serilog;

namespace MyWallet3.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.ConfigureSerilog();
            builder.Services.AddProjectServices(builder.Configuration);
            builder.Services.AddJwtAuthentication(builder.Configuration);
            builder.Services.AddSwaggerDocumentation();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

            app.UseSerilogRequestLogging();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var connectionString = config.GetConnectionString("DefaultConnection");

                MyWallet3.Infrastructure.Database.DatabaseMigrator.Migrate(connectionString!);
            }

            app.Run();
        }
    }
}
