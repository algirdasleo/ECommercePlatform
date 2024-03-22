using SharedLibrary.Interfaces;
using SharedLibrary.Services;
using Product.Models;
using Dapper;

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
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var products = await connection.QueryAsync<ProductItem>("SELECT * FROM products");
                return products.ToList();
            }
        }

        public async Task<ProductItem?> GetByIdAsync(int id)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var product = await connection.QueryFirstOrDefaultAsync<ProductItem>("SELECT * FROM products WHERE ProductId = @id", new { id });
                return product;
            }
        }

        public async Task<ProductItem> CreateAsync(ProductItem product)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                string sqlQuery = @"
                    INSERT INTO products (ProductName, Description, Price, CategoryId, CreatedAt)
                    VALUES (@ProductName, @Description, @Price, @CategoryId, @CreatedAt)
                    RETURNING *";
                var createdProduct = await connection.QueryFirstOrDefaultAsync<ProductItem>(sqlQuery, product);
                if (createdProduct == null)
                    throw new Exception("ProductItem not created");
                return createdProduct;
            }   
        }

        public async Task<ProductItem> UpdateAsync(ProductItem product)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                string sqlQuery = @"
                    UPDATE products 
                    SET ProductName = @ProductName,
                        Description = @Description, Price = @Price,
                        CategoryId = @CategoryId
                    WHERE ProductId = @ProductId
                    RETURNING *";
                var updatedProduct = await connection.QueryFirstOrDefaultAsync<ProductItem>(sqlQuery, product);
                if (updatedProduct == null)
                    throw new Exception("ProductItem not updated");
                return updatedProduct;
            }
        }

        public async Task<ProductItem> DeleteAsync(int id)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                string sqlQuery = @"
                    DELETE FROM products
                    WHERE ProductId = @id
                    RETURNING *";
                var deletedProduct = await connection.QueryFirstOrDefaultAsync<ProductItem>(sqlQuery, new { id });
                if (deletedProduct == null)
                    throw new Exception("ProductItem not deleted");
                return deletedProduct;
            }
        }
    }
}


