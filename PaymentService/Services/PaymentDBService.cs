using SharedLibrary.Interfaces;
using SharedLibrary.Services;
using PaymentService.Models;
using Dapper;

namespace PaymentService.Services
{
    public class PaymentDBService : IDBService<Payment>
    {
        private readonly DBConnectionFactory _dbConnectionFactory;

        public PaymentDBService(DBConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<List<Payment>> GetAllAsync()
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var payments = await connection.QueryAsync<Payment>("SELECT * FROM payments");
                return payments.ToList();
            }
        }

        public async Task<Payment?> GetByIdAsync(int id)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var payment = await connection.QueryFirstOrDefaultAsync<Payment>("SELECT * FROM payments WHERE paymentId = @id", new { id });
                return payment;
            }
        }

        public async Task<Payment> CreateAsync(Payment payment)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var sqlQuery = @"
                    INSERT INTO payments (OrderId, Amount, PaymentDate, PaymentMethod)
                    VALUES (@OrderId, @Amount, @PaymentDate, @PaymentMethod)
                    RETURNING *";
                var createdPayment = await connection.QueryFirstOrDefaultAsync<Payment>(sqlQuery, payment);
                if (createdPayment == null)
                    throw new Exception("Payment not created");
                return createdPayment;
            }
        }

        public async Task<Payment> UpdateAsync(Payment payment)
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
                var updatedPayment = await connection.QueryFirstOrDefaultAsync<Payment>(sqlQuery, payment);
                if (updatedPayment == null)
                    throw new Exception("Payment not updated");
                return updatedPayment;
            }
        }

        public async Task<Payment> DeleteAsync(int id)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var sqlQuery = @"
                DELETE FROM payments
                WHERE PaymentId = @id";
                var deletedProduct = await connection.QueryFirstOrDefaultAsync<Payment>(sqlQuery, new { id });
                if (deletedProduct == null)
                    throw new Exception("Payment not updated");
                return deletedProduct;
            }
        }
    }
}