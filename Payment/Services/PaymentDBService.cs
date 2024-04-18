using SharedLibrary.Interfaces;
using SharedLibrary.Services;
using Payment.Models;
using Dapper;
using SharedLibrary.Helpers;

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
            var sql = ResourceHelper.GetQuery("PaymentGetAllAsync");
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var payments = await connection.QueryAsync<PaymentItem>(sql);
                return payments.ToList();
            }
        }

        public async Task<PaymentItem?> GetByIdAsync(int id)
        {
            var sql = ResourceHelper.GetQuery("PaymentGetByIdAsync");
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var payment = await connection.QueryFirstOrDefaultAsync<PaymentItem>(sql, new { id });
                return payment;
            }
        }

        public async Task<PaymentItem> CreateAsync(PaymentItem payment)
        {
            var sql = ResourceHelper.GetQuery("PaymentCreateAsync");
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var createdPayment = await connection.QueryFirstOrDefaultAsync<PaymentItem>(sql, payment);
                if (createdPayment == null)
                    throw new Exception("PaymentItem not created");
                return createdPayment;
            }
        }

        public async Task<PaymentItem> UpdateAsync(PaymentItem payment)
        {
            var sql = ResourceHelper.GetQuery("PaymentUpdateAsync");
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var updatedPayment = await connection.QueryFirstOrDefaultAsync<PaymentItem>(sql, payment);
                if (updatedPayment == null)
                    throw new Exception("PaymentItem not updated");
                return updatedPayment;
            }
        }

        public async Task<PaymentItem> DeleteAsync(int id)
        {
            var sql = ResourceHelper.GetQuery("PaymentDeleteAsync");
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var deletedProduct = await connection.QueryFirstOrDefaultAsync<PaymentItem>(sql, new { id });
                if (deletedProduct == null)
                    throw new Exception("PaymentItem not updated");
                return deletedProduct;
            }
        }
    }
}