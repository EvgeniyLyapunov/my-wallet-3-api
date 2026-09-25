using MyWallet3.Application.Interfaces.Auth;
using MyWallet3.Application.Interfaces.Repositories;
using MyWallet3.Application.Interfaces.Services;
using MyWallet3.Application.Services.Auth;
using MyWallet3.Infrastructure.Auth;
using MyWallet3.Infrastructure.Database;
using MyWallet3.Infrastructure.Repositories;

namespace MyWallet3.API.Extensions;

public static class DependencyInjectionExtensions
{
    public static void AddProjectServices(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Привязываем настройки JWT из appsettings.json к классу JwtOptions
        services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));

        // 2. Регистрируем фабрику подключений к БД (Singleton - один экземпляр на всё приложение)
        services.AddSingleton<NpgsqlConnectionFactory>();

        // 3. Регистрируем репозитории (Scoped - создаются заново для каждого HTTP запроса)
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        // 4. Регистрируем механизмы инфраструктуры
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtProvider, JwtProvider>();

        // 5. Регистрируем саму бизнес-логику
        services.AddScoped<IAuthService, AuthService>();
    }
}
