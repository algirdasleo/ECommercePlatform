using SharedLibrary.Interfaces;
using SharedLibrary.Services;
using ProductService.Models;
using Dapper;

namespace ProductService.Services
{
    public class ProductDBService : IDBService<Product>
    {
        private readonly DBConnectionFactory _dbConnectionFactory;

        public ProductDBService (DBConnectionFactory dBConnectionFactory)
        {
            _dbConnectionFactory = dBConnectionFactory;
        }
        
        public async Task<List<Product>> GetAllAsync()
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var products = await connection.QueryAsync<Product>("SELECT * FROM products");
                return products.ToList();
            }
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var product = await connection.QueryFirstOrDefaultAsync<Product>("SELECT * FROM products WHERE ProductId = @id", new { id });
                return product;
            }
        }

        public async Task<Product> CreateAsync(Product product)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                string sqlQuery = @"
                    INSERT INTO products (ProductId, ProductName, Description, Price, CategoryId, CreatedAt)
                    VALUES (@ProductId, @ProductName, @Description, @Price, @CategoryId, @CreatedAt)
                    RETURNING *";
                var createdProduct = await connection.QueryFirstOrDefaultAsync<Product>(sqlQuery, product);
                if (createdProduct == null)
                    throw new Exception("Product not created");
                return createdProduct;
            }   
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                string sqlQuery = @"
                    UPDATE products 
                    SET ProductId = @ProductId, ProductName = @ProductName,
                        Description = @Description, Price = @Price,
                        CategoryId = @CategoryId, CreatedAt = @CreatedAt
                    RETURNING *";
                var updatedProduct = await connection.QueryFirstOrDefaultAsync<Product>(sqlQuery, product);
                if (updatedProduct == null)
                    throw new Exception("Product not updated");
                return updatedProduct;
            }
        }

        public async Task<Product> DeleteAsync(int id)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                string sqlQuery = @"
                    DELETE FROM products
                    WHERE ProductId = @id
                    RETURNING *";
                var deletedProduct = await connection.QueryFirstOrDefaultAsync<Product>(sqlQuery, new { id });
                if (deletedProduct == null)
                    throw new Exception("Product not deleted");
                return deletedProduct;
            }
        }
    }
}


