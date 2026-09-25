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
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly NpgsqlConnectionFactory _connectionFactory;

        public RefreshTokenRepository(NpgsqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task CreateAsync(RefreshToken token)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                string sql = @"
INSERT INTO refresh_tokens (id, user_id, token, expires_at) 
VALUES (@Id, @UserId, @Token, @ExpiresAt)";

                var parameters = new DynamicParameters();
                parameters.Add("@Id", token.Id);
                parameters.Add("@UserId", token.UserId);
                parameters.Add("@Token", token.Token);
                parameters.Add("@ExpiresAt", token.ExpiresAt);

                await connection.ExecuteAsync(sql, parameters);
            }

        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                string sql = @"
SELECT 
    id as Id, 
    user_id as UserId, 
    token as Token, 
    expires_at as ExpiresAt 
FROM refresh_tokens 
WHERE token = @Token";

                var parameters = new DynamicParameters();
                parameters.Add("@Token", token);

                return await connection.QuerySingleOrDefaultAsync<RefreshToken>(sql, parameters);
            }

        }

        public async Task DeleteAsync(Guid tokenId)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                string sql = @"DELETE FROM refresh_tokens WHERE id = @Id";

                var parameters = new DynamicParameters();
                parameters.Add("@Id", tokenId);

                await connection.ExecuteAsync(sql, parameters);
            }
        }
    }
}
