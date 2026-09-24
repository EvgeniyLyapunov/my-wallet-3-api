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
        Task RegisterAsync(string login, string password);
        Task<JwtTokens> LoginAsync(string login, string password);
        Task<JwtTokens> RefreshTokensAsync(string oldRefreshToken);
        Task LogoutAsync(string refreshToken);
    }
}
