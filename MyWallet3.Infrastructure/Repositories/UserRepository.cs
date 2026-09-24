using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MyWallet3.Application.Interfaces.Repositories;
using MyWallet3.Domain.Users;
using MyWallet3.Infrastructure.Database;

namespace MyWallet3.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly NpgsqlConnectionFactory _connectionFactory;

        public UserRepository(NpgsqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<User?> GetByLoginAsync(string login)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                string sql = @"
SELECT 
    id as Id,
    login as Login, 
    password_hash as PasswordHash 
FROM users 
WHERE login = @Login";

                var parameters = new DynamicParameters();
                parameters.Add("@Login", login);

                return await connection.QuerySingleOrDefaultAsync<User>(sql, parameters);
            }
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                string sql = @"
SELECT id as Id, 
login as Login, 
password_hash as PasswordHash 
FROM users 
WHERE id = @Id";

                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);

                return await connection.QuerySingleOrDefaultAsync<User>(sql, parameters);
            }

        }

        public async Task CreateAsync(User user)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                string sql = @"
INSERT INTO users (id, login, password_hash) 
VALUES (@Id, @Login, @PasswordHash)";

                var parameters = new DynamicParameters();
                parameters.Add("@Id", user.Id);
                parameters.Add("@Login", user.Login);
                parameters.Add("@PasswordHash", user.PasswordHash);

                await connection.ExecuteAsync(sql, parameters);
            }
        }
    }
}
