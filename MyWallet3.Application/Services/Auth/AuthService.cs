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
            // 1. Проверяем, нет ли уже такого пользователя
            var existingUser = await _userRepository.GetByLoginAsync(login);
            if (existingUser != null)
            {
                throw new Exception("Пользователь с таким логином уже существует");
            }

            // 2. Создаем пользователя, хэшируя пароль
            var user = new User
            {
                Id = Guid.NewGuid(),
                Login = login,
                PasswordHash = _passwordHasher.Generate(password)
            };

            // 3. Сохраняем в БД
            await _userRepository.CreateAsync(user);
        }

        public async Task<JwtTokens> LoginAsync(string login, string password)
        {
            // 1. Ищем пользователя
            var user = await _userRepository.GetByLoginAsync(login);
            if (user == null)
            {
                throw new Exception("Неверный логин или пароль");
            }

            // 2. Проверяем пароль
            var isPasswordValid = _passwordHasher.Verify(password, user.PasswordHash);
            if (!isPasswordValid)
            {
                throw new Exception("Неверный логин или пароль");
            }

            // 3. Генерируем токены
            var tokens = _jwtProvider.GenerateTokens(user);

            // 4. Запоминаем рефреш-токен в базу (чтобы потом можно было его проверить)
            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Token = tokens.RefreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(30) // Выдаем на 30 дней
            };
            await _refreshTokenRepository.CreateAsync(refreshToken);

            // 5. Отдаем токены наружу
            return tokens;
        }
    }
}
