using System.Security.Cryptography;
using System.Text;
using MyWallet3.Application.Interfaces.Auth;

namespace MyWallet3.Infrastructure.Auth;

public class PasswordHasher : IPasswordHasher
{
    public string Generate(string password)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);

        var hashBytes = SHA256.HashData(passwordBytes);

        return Convert.ToBase64String(hashBytes);
    }

    public bool Verify(string password, string hashedPassword)
    {
        var hashOfInput = Generate(password);

        return hashOfInput == hashedPassword;
    }
}
