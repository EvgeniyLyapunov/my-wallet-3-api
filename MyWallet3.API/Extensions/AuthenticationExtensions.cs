using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace MyWallet3.API.Extensions;

public static class AuthenticationExtensions
{
    public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var secretKey = configuration["JwtOptions:SecretKey"]
            ?? throw new Exception("Секретный ключ не найден в конфигурации!");

        // Настраиваем правила проверки
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false, 
                    ValidateAudience = false, 
                    ValidateLifetime = true, // Проверять срок годности (Expires)
                    ValidateIssuerSigningKey = true, // Проверять саму крипто-подпись
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ClockSkew = TimeSpan.Zero 
                };
            });

        // Добавляем поддержку атрибута [Authorize]
        services.AddAuthorization();
    }
}
