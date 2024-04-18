using SharedLibrary.Interfaces;
using SharedLibrary.Services;
using Order.Models;
using Dapper;
using SharedLibrary.Helpers;

namespace Order.Services
{
    public class OrderDBService : IDBService<OrderItem>
    {
        private readonly DBConnectionFactory _dbConnectionFactory;

        public OrderDBService(DBConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }
        
        public async Task<List<OrderItem>> GetAllAsync()
        {
            var sql = ResourceHelper.GetQuery("OrderGetAllAsync");
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var lists = await connection.QueryAsync<OrderItem>(sql);
                return lists.ToList();
            }
        }

        public async Task<OrderItem?> GetByIdAsync(int id)
        {
            var sql = ResourceHelper.GetQuery("OrderGetByIdAsync");
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var order = await connection.QueryFirstOrDefaultAsync<OrderItem>(sql, new { id });
                return order;
            }
        }

        public async Task<OrderItem> CreateAsync(OrderItem order)
        {
            var sql = ResourceHelper.GetQuery("OrderCreateAsync");
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var createdOrder = await connection.QueryFirstOrDefaultAsync<OrderItem>(sql, order);
                if (createdOrder == null)
                    throw new Exception("OrderItem not created");
                return createdOrder;
            }
        }

        public async Task<OrderItem> UpdateAsync(OrderItem order)
        {
            var sql = ResourceHelper.GetQuery("OrderUpdateAsync");
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var updatedOrder = await connection.QueryFirstOrDefaultAsync<OrderItem>(sql, order);
                if (updatedOrder == null)
                    throw new Exception("OrderItem not updated");
                return updatedOrder;
            }
        }

        public async Task<OrderItem> DeleteAsync(int id)
        {
            var sql = ResourceHelper.GetQuery("OrderDeleteAsync");
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var deletedOrder = await connection.QueryFirstOrDefaultAsync<OrderItem>(sql, new { id });
                if (deletedOrder == null)
                    throw new Exception("OrderItem not deleted");
                return deletedOrder;
            }
        }

    }
}