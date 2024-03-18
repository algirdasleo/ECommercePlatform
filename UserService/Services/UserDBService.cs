using SharedLibrary.Interfaces;
using Dapper;
using Npgsql;
using UserService.Models;
using System.Data.Common;
using SharedLibrary.Services;

namespace UserService.Services
{
    public class UserDBService : IDBService<User>
    {
        private readonly DBConnectionFactory _dbConnectionFactory;

        public UserDBService(DBConnectionFactory dBConnectionFactory)
        {
            _dbConnectionFactory = dBConnectionFactory;
        }

        public async Task<List<User>> GetAllAsync()
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var users = await connection.QueryAsync<User>("SELECT * FROM users");
                return users.ToList();
            }
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var user = await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM users WHERE userId = @id", new { id });
                return user;
            }
        }

        public async Task<User> CreateAsync(User user)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                string sqlQuery = @"
                    INSERT INTO users (UserName, Email, Password, RegistrationDate, UserType)
                    VALUES (@UserName, @Email, @Password, @RegistrationDate, @UserType)
                    RETURNING *";
                var createdUser = await connection.QueryFirstOrDefaultAsync<User>(sqlQuery, user);
                if (createdUser == null)
                    throw new Exception("User not created");
                return createdUser;
            }
        }
        public async Task<User> UpdateAsync(User user)
        {
            if (GetByIdAsync(user.UserId) == null)
                throw new Exception("User not found");
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                string sqlQuery = @"
                    UPDATE users
                    SET UserName = @UserName, Email = @Email, Password = @Password, RegistrationDate = @RegistrationDate, UserType = @UserType
                    WHERE UserId = @UserId
                    RETURNING *";
                var updatedUser = await connection.QueryFirstOrDefaultAsync<User>(sqlQuery, user);
                if (updatedUser == null)
                    throw new Exception("User not found");
                return updatedUser;
            }
        }
        public async Task<User> DeleteAsync(int id)
        {
            if (GetByIdAsync(id) == null)
                throw new Exception("User not found");
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                string sqlQuery = @"
                    DELETE FROM users
                    WHERE UserId = @id
                    RETURNING *";
                var deletedUser = await connection.QueryFirstOrDefaultAsync<User>(sqlQuery, new { id });
                if (deletedUser == null)
                    throw new Exception("User not found");
                return deletedUser;
            }
        }
    }
}