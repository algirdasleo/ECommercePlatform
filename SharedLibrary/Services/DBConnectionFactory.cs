using System.Data.Common;
using System.Threading.Tasks;
using Npgsql;

namespace SharedLibrary.Services
{
    public class DBConnectionFactory 
    {
        
        private readonly string _connectionString;

        public DBConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<DbConnection> CreateConnectionAsync()
        {
            var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}