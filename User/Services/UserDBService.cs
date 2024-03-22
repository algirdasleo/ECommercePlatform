using SharedLibrary.Interfaces;
using Dapper;
using User.Models;
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
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var users = await connection.QueryAsync<UserItem>("SELECT * FROM users");
                return users.ToList();
            }
        }

        public async Task<UserItem?> GetByIdAsync(int id)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var user = await connection.QueryFirstOrDefaultAsync<UserItem>("SELECT * FROM users WHERE userId = @id", new { id });
                return user;
            }
        }

        public async Task<UserItem> CreateAsync(UserItem user)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                string sqlQuery = @"
                    INSERT INTO users (UserName, Email, Password, RegistrationDate, UserType)
                    VALUES (@UserName, @Email, @Password, @RegistrationDate, @UserType)
                    RETURNING *";
                var createdUser = await connection.QueryFirstOrDefaultAsync<UserItem>(sqlQuery, user);
                if (createdUser == null)
                    throw new Exception("UserItem not created");
                return createdUser;
            }
        }
        public async Task<UserItem> UpdateAsync(UserItem user)
        {
            if (GetByIdAsync(user.UserId) == null)
                throw new Exception("UserItem not found");
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                string sqlQuery = @"
                    UPDATE users
                    SET UserName = @UserName, Email = @Email, Password = @Password, RegistrationDate = @RegistrationDate, UserType = @UserType
                    WHERE UserId = @UserId
                    RETURNING *";
                var updatedUser = await connection.QueryFirstOrDefaultAsync<UserItem>(sqlQuery, user);
                if (updatedUser == null)
                    throw new Exception("UserItem not found");
                return updatedUser;
            }
        }
        public async Task<UserItem> DeleteAsync(int id)
        {
            if (GetByIdAsync(id) == null)
                throw new Exception("UserItem not found");
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                string sqlQuery = @"
                    DELETE FROM users
                    WHERE UserId = @id
                    RETURNING *";
                var deletedUser = await connection.QueryFirstOrDefaultAsync<UserItem>(sqlQuery, new { id });
                if (deletedUser == null)
                    throw new Exception("UserItem not found");
                return deletedUser;
            }
        }
    }
}