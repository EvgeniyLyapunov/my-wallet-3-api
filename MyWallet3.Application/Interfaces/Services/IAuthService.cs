using MyWallet3.Application.Interfaces.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet3.Application.Interfaces.Services
{
    public interface IAuthService
    {
        // Метод регистрации
        Task RegisterAsync(string login, string password);

        // Метод логина (возвращает пару токенов)
        Task<JwtTokens> LoginAsync(string login, string password);
    }
}
