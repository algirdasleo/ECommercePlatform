using SharedLibrary.Interfaces;
using SharedLibrary.Services;
using Product.Models;
using Dapper;
using SharedLibrary.Helpers;

namespace Product.Services
{
    public class ProductDBService : IDBService<ProductItem>
    {
        private readonly DBConnectionFactory _dbConnectionFactory;

        public ProductDBService (DBConnectionFactory dBConnectionFactory)
        {
            _dbConnectionFactory = dBConnectionFactory;
        }
        
        public async Task<List<ProductItem>> GetAllAsync()
        {
            var sql = ResourceHelper.GetQuery("ProductGetAllAsync");
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var products = await connection.QueryAsync<ProductItem>(sql);
                return products.ToList();
            }
        }

        public async Task<ProductItem?> GetByIdAsync(int id)
        {
            var sql = ResourceHelper.GetQuery("ProductGetByIdAsync");
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var product = await connection.QueryFirstOrDefaultAsync<ProductItem>(sql, new { id });
                return product;
            }
        }

        public async Task<ProductItem> CreateAsync(ProductItem product)
        {
            var sql = ResourceHelper.GetQuery("ProductCreateAsync");
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var createdProduct = await connection.QueryFirstOrDefaultAsync<ProductItem>(sql, product);
                if (createdProduct == null)
                    throw new Exception("ProductItem not created");
                return createdProduct;
            }   
        }

        public async Task<ProductItem> UpdateAsync(ProductItem product)
        {
            var sql = ResourceHelper.GetQuery("ProductUpdateAsync");
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var updatedProduct = await connection.QueryFirstOrDefaultAsync<ProductItem>(sql, product);
                if (updatedProduct == null)
                    throw new Exception("ProductItem not updated");
                return updatedProduct;
            }
        }

        public async Task<ProductItem> DeleteAsync(int id)
        {
            var sql = ResourceHelper.GetQuery("ProductDeleteAsync");
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var deletedProduct = await connection.QueryFirstOrDefaultAsync<ProductItem>(sql, new { id });
                if (deletedProduct == null)
                    throw new Exception("ProductItem not deleted");
                return deletedProduct;
            }
        }
    }
}


