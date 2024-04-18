using SharedLibrary.Interfaces;
using Dapper;
using User.Models;
using SharedLibrary.Helpers;
using SharedLibrary.Services;

namespace User.Services
{
    public class UserDBService : IDBService<UserItem>
    {
        private readonly DBConnectionFactory _dbConnectionFactory;

        public UserDBService(DBConnectionFactory dBConnectionFactory)
        {
            _dbConnectionFactory = dBConnectionFactory;
        }

        public async Task<List<UserItem>> GetAllAsync()
        {
            var sql = ResourceHelper.GetQuery("UserGetAllAsync");
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var users = await connection.QueryAsync<UserItem>("");
                return users.ToList();
            }
        }

        public async Task<UserItem?> GetByIdAsync(int id)
        {
            var sql = ResourceHelper.GetQuery("UserGetByIdAsync");
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var user = await connection.QueryFirstOrDefaultAsync<UserItem>(sql, new { id });
                return user;
            }
        }

        public async Task<UserItem> CreateAsync(UserItem user)
        {
            var sql = ResourceHelper.GetQuery("UserCreateAsync");
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var createdUser = await connection.QueryFirstOrDefaultAsync<UserItem>(sql, user);
                if (createdUser == null)
                    throw new Exception("UserItem not created");
                return createdUser;
            }
        }
        public async Task<UserItem> UpdateAsync(UserItem user)
        {
            var sqlQuery = ResourceHelper.GetQuery("UserUpdateAsync");
            if (GetByIdAsync(user.UserId) == null)
                throw new Exception("UserItem not found");
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var updatedUser = await connection.QueryFirstOrDefaultAsync<UserItem>(sqlQuery, user);
                if (updatedUser == null)
                    throw new Exception("UserItem not found");
                return updatedUser;
            }
        }
        public async Task<UserItem> DeleteAsync(int id)
        {
            var sql = ResourceHelper.GetQuery("UserDeleteAsync");
            if (GetByIdAsync(id) == null)
                throw new Exception("UserItem not found");
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {

                var deletedUser = await connection.QueryFirstOrDefaultAsync<UserItem>(sql, new { id });
                if (deletedUser == null)
                    throw new Exception("UserItem not found");
                return deletedUser;
            }
        }
    }
}