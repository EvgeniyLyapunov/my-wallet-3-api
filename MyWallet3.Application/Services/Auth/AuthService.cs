using MyWallet3.Application.Interfaces.Auth;
using MyWallet3.Application.Interfaces.Repositories;
using MyWallet3.Application.Interfaces.Services;
using MyWallet3.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet3.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;

        // Через конструктор мы получаем реализации (сотрудников) для наших контрактов
        public AuthService(
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IPasswordHasher passwordHasher,
            IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }

        public async Task RegisterAsync(string login, string password)
        {
            var existingUser = await _userRepository.GetByLoginAsync(login);
            if (existingUser != null)
            {
                throw new Exception("Пользователь с таким логином уже существует");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Login = login,
                PasswordHash = _passwordHasher.Generate(password)
            };

            await _userRepository.CreateAsync(user);
        }

        public async Task<JwtTokens> LoginAsync(string login, string password)
        {
            var user = await _userRepository.GetByLoginAsync(login);
            if (user == null)
            {
                throw new Exception("Неверный логин или пароль");
            }

            var isPasswordValid = _passwordHasher.Verify(password, user.PasswordHash);
            if (!isPasswordValid)
            {
                throw new Exception("Неверный логин или пароль");
            }

            var tokens = _jwtProvider.GenerateTokens(user);

            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Token = tokens.RefreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(30) 
            };
            await _refreshTokenRepository.CreateAsync(refreshToken);

            return tokens;
        }

        public async Task<JwtTokens> RefreshTokensAsync(string oldRefreshToken)
        {
            var tokenRecord = await _refreshTokenRepository.GetByTokenAsync(oldRefreshToken);

            if (tokenRecord == null || tokenRecord.ExpiresAt < DateTime.UtcNow)
            {
                throw new Exception("Токен недействителен или просрочен");
            }

            var user = await _userRepository.GetByIdAsync(tokenRecord.UserId);
            if (user == null)
            {
                throw new Exception("Пользователь не найден");
            }

            var newTokens = _jwtProvider.GenerateTokens(user);

            var newRefreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Token = newTokens.RefreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(30)
            };
            await _refreshTokenRepository.CreateAsync(newRefreshToken);

            await _refreshTokenRepository.DeleteAsync(tokenRecord.Id);

            return newTokens;
        }

        public async Task LogoutAsync(string refreshToken)
        {
            var tokenRecord = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

            if (tokenRecord != null)
            {
                await _refreshTokenRepository.DeleteAsync(tokenRecord.Id);
            }
        }
    }
}
