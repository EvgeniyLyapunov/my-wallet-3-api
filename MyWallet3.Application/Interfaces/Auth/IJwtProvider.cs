using MyWallet3.Domain.Users;

namespace MyWallet3.Application.Interfaces.Auth
{
    public interface IJwtProvider
    {
        // Метод получает на вход Пользователя и возвращает парочку токенов
        JwtTokens GenerateTokens(User user);
    }
}
