using SharedLibrary.Interfaces;
using SharedLibrary.Services;
using OrderService.Models;
using Dapper;

namespace OrderService.Services
{
    public class OrderDBService : IDBService<Order>
    {
        private readonly DBConnectionFactory _dbConnectionFactory;

        public OrderDBService(DBConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }
        
        public async Task<List<Order>> GetAllAsync()
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var lists = await connection.QueryAsync<Order>("SELECT * FROM orders");
                return lists.ToList();
            }
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var order = await connection.QueryFirstOrDefaultAsync<Order>("SELECT * FROM orders WHERE orderId = @id", new { id });
                return order;
            }
        }

        public async Task<Order> CreateAsync(Order order)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var sqlQuery = @"
                    INSERT INTO orders (OrderId, UserId, OrderDate, TotalAmount, OrderStatus)
                    VALUES (@OrderId, @UserId, @OrderDate, @TotalAmount, @OrderStatus)
                    RETURNING *";
                var createdOrder = await connection.QueryFirstOrDefaultAsync<Order>(sqlQuery, order);
                if (createdOrder == null)
                    throw new Exception("Order not created");
                return createdOrder;
            }
        }

        public async Task<Order> UpdateAsync(Order order)
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
                var updatedOrder = await connection.QueryFirstOrDefaultAsync<Order>(sqlQuery, order);
                if (updatedOrder == null)
                    throw new Exception("Order not updated");
                return updatedOrder;
            }
        }

        public async Task<Order> DeleteAsync(int id)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var sqlQuery = @"
                    DELETE FROM orders
                    WHERE orderId = @id
                    RETURNING *";
                var deletedOrder = await connection.QueryFirstOrDefaultAsync<Order>(sqlQuery, new { id });
                if (deletedOrder == null)
                    throw new Exception("Order not deleted");
                return deletedOrder;
            }
        }

    }
}