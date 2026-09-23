using MyWallet3.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet3.Application.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        // Сохранить выданный рефреш-токен в базу
        Task CreateAsync(RefreshToken token);

        // Найти токен в базе по самой строке токена
        Task<RefreshToken?> GetByTokenAsync(string token);

        // Удалить токен из базы (при логауте)
        Task DeleteAsync(Guid tokenId);
    }
}
