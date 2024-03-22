using SharedLibrary.Interfaces;
using SharedLibrary.Services;
using Payment.Models;
using Dapper;

namespace Payment.Services
{
    public class PaymentDBService : IDBService<PaymentItem>
    {
        private readonly DBConnectionFactory _dbConnectionFactory;

        public PaymentDBService(DBConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<List<PaymentItem>> GetAllAsync()
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var payments = await connection.QueryAsync<PaymentItem>("SELECT * FROM payments");
                return payments.ToList();
            }
        }

        public async Task<PaymentItem?> GetByIdAsync(int id)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var payment = await connection.QueryFirstOrDefaultAsync<PaymentItem>("SELECT * FROM payments WHERE paymentId = @id", new { id });
                return payment;
            }
        }

        public async Task<PaymentItem> CreateAsync(PaymentItem payment)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var sqlQuery = @"
                    INSERT INTO payments (OrderId, Amount, PaymentDate, PaymentMethod)
                    VALUES (@OrderId, @Amount, @PaymentDate, @PaymentMethod)
                    RETURNING *";
                var createdPayment = await connection.QueryFirstOrDefaultAsync<PaymentItem>(sqlQuery, payment);
                if (createdPayment == null)
                    throw new Exception("PaymentItem not created");
                return createdPayment;
            }
        }

        public async Task<PaymentItem> UpdateAsync(PaymentItem payment)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var sqlQuery = @"
                UPDATE payments
                SET OrderId = @OrderId,
                    Amount = @Amount, PaymentDate = @PaymentDate,
                    PaymentMethod = @PaymentMethod
                WHERE PaymentId = @PaymentId
                RETURNING *";
                var updatedPayment = await connection.QueryFirstOrDefaultAsync<PaymentItem>(sqlQuery, payment);
                if (updatedPayment == null)
                    throw new Exception("PaymentItem not updated");
                return updatedPayment;
            }
        }

        public async Task<PaymentItem> DeleteAsync(int id)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var sqlQuery = @"
                DELETE FROM payments
                WHERE PaymentId = @id";
                var deletedProduct = await connection.QueryFirstOrDefaultAsync<PaymentItem>(sqlQuery, new { id });
                if (deletedProduct == null)
                    throw new Exception("PaymentItem not updated");
                return deletedProduct;
            }
        }
    }
}