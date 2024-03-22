using SharedLibrary.Interfaces;
using SharedLibrary.Services;
using Order.Models;
using Dapper;

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
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var lists = await connection.QueryAsync<OrderItem>("SELECT * FROM orders");
                return lists.ToList();
            }
        }

        public async Task<OrderItem?> GetByIdAsync(int id)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var order = await connection.QueryFirstOrDefaultAsync<OrderItem>("SELECT * FROM orders WHERE orderId = @id", new { id });
                return order;
            }
        }

        public async Task<OrderItem> CreateAsync(OrderItem order)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var sqlQuery = @"
                    INSERT INTO orders (UserId, OrderDate, TotalAmount, OrderStatus)
                    VALUES (@UserId, @OrderDate, @TotalAmount, @OrderStatus)
                    RETURNING *";
                var createdOrder = await connection.QueryFirstOrDefaultAsync<OrderItem>(sqlQuery, order);
                if (createdOrder == null)
                    throw new Exception("OrderItem not created");
                return createdOrder;
            }
        }

        public async Task<OrderItem> UpdateAsync(OrderItem order)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var sqlQuery = @"
                    UPDATE orders
                    SET UserId = @UserId, 
                        OrderDate = @OrderDate, TotalAmount = @TotalAmount, 
                        OrderStatus = @OrderStatus
                    WHERE OrderId = @OrderId
                    RETURNING *";
                var updatedOrder = await connection.QueryFirstOrDefaultAsync<OrderItem>(sqlQuery, order);
                if (updatedOrder == null)
                    throw new Exception("OrderItem not updated");
                return updatedOrder;
            }
        }

        public async Task<OrderItem> DeleteAsync(int id)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var sqlQuery = @"
                    DELETE FROM orders
                    WHERE orderId = @id
                    RETURNING *";
                var deletedOrder = await connection.QueryFirstOrDefaultAsync<OrderItem>(sqlQuery, new { id });
                if (deletedOrder == null)
                    throw new Exception("OrderItem not deleted");
                return deletedOrder;
            }
        }

    }
}