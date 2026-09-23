using MyWallet3.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet3.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        // Найти пользователя по логину (может вернуть null, если такого нет)
        Task<User?> GetByLoginAsync(string login);

        // Сохранить нового пользователя в базу
        Task CreateAsync(User user);
    }
}
