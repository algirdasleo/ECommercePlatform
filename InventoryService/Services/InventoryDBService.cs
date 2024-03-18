using SharedLibrary.Interfaces;
using SharedLibrary.Services;
using InventoryService.Models;
using Dapper;

namespace InventoryService.Services
{
    public class InventoryDBService : IDBService<InventoryItem>
    {
        private readonly DBConnectionFactory _dbConnectionFactory;

        public InventoryDBService(DBConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<List<InventoryItem>> GetAllAsync()
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var inventoryItems = await connection.QueryAsync<InventoryItem>("SELECT * FROM inventoryItems");
                return inventoryItems.ToList();
            }
        }

        public async Task<InventoryItem?> GetByIdAsync(int id)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var inventoryItem = await connection.QueryFirstOrDefaultAsync<InventoryItem>("SELECT * FROM inventoryItems WHERE inventoryItemId = @id", new { id });
                return inventoryItem;
            }
        }

        public async Task<InventoryItem> CreateAsync(InventoryItem inventoryItem)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var sqlQuery = @"
                    INSERT INTO inventoryItems (InventoryItemId, ProductId, QuantityAvailable)
                    VALUES (@InventoryItemId, @ProductId, @QuantityAvailable)
                    RETURNING *";
                var createdInventoryItem = await connection.QueryFirstOrDefaultAsync<InventoryItem>(sqlQuery, inventoryItem);
                if (createdInventoryItem == null)
                    throw new Exception("InventoryItem not created");
                return createdInventoryItem;
            }
        }

        public async Task<InventoryItem> UpdateAsync(InventoryItem inventoryItem)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var sqlQuery = @"
                    UPDATE inventoryItems
                    SET ProductId = @ProductId,
                        QuantityAvailable = @QuantityAvailable
                    WHERE InventoryItemId = @InventoryItemId
                    RETURNING *";
                var updatedInventoryItem = await connection.QueryFirstOrDefaultAsync<InventoryItem>(sqlQuery, inventoryItem);
                if (updatedInventoryItem == null)
                    throw new Exception("InventoryItem not updated");
                return updatedInventoryItem;
            }
        }

        public async Task<InventoryItem> DeleteAsync(int id)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var sqlQuery = @"
                    DELETE FROM inventoryItems
                    WHERE inventoryItemId = @id
                    RETURNING *";
                var deletedInventoryItem = await connection.QueryFirstOrDefaultAsync<InventoryItem>(sqlQuery, new { id });
                if (deletedInventoryItem == null)
                    throw new Exception("InventoryItem not deleted");
                return deletedInventoryItem;
            }
        }
    }
}