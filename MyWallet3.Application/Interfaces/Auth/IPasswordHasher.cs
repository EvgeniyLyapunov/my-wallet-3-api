namespace MyWallet3.Application.Interfaces.Auth
{
    public interface IPasswordHasher
    {
        // Метод для создания хэша из пароля при регистрации
        string Generate(string password);

        // Метод для проверки пароля при логине
        bool Verify(string password, string hashedPassword);
    }
}
