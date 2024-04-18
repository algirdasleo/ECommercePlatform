using SharedLibrary.Interfaces;
using SharedLibrary.Services;
using SharedLibrary.Helpers;
using Inventory.Models;
using Dapper;

namespace Inventory.Services
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
            string sql = ResourceHelper.GetQuery("InventoryGetAllAsync");
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var inventoryItems = await connection.QueryAsync<InventoryItem>(sql);
                return inventoryItems.ToList();
            }
        }

        public async Task<InventoryItem?> GetByIdAsync(int id)
        {
            string sql = ResourceHelper.GetQuery("InventoryGetByIdAsync");
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var inventoryItem = await connection.QueryFirstOrDefaultAsync<InventoryItem>(sql, new { id });
                return inventoryItem;
            }
        }

        public async Task<InventoryItem> CreateAsync(InventoryItem inventoryItem)
        {
            var sql = ResourceHelper.GetQuery("InventoryCreateAsync");
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var createdInventoryItem = await connection.QueryFirstOrDefaultAsync<InventoryItem>(sql, inventoryItem);
                if (createdInventoryItem == null)
                    throw new Exception("InventoryItem not created");
                return createdInventoryItem;
            }
        }

        public async Task<InventoryItem> UpdateAsync(InventoryItem inventoryItem)
        {
            var sql = ResourceHelper.GetQuery("InventoryUpdateAsync");
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var updatedInventoryItem = await connection.QueryFirstOrDefaultAsync<InventoryItem>(sql, inventoryItem);
                if (updatedInventoryItem == null)
                    throw new Exception("InventoryItem not updated");
                return updatedInventoryItem;
            }
        }

        public async Task<InventoryItem> DeleteAsync(int id)
        {
            var sql = ResourceHelper.GetQuery("InventoryDeleteAsync");
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var deletedInventoryItem = await connection.QueryFirstOrDefaultAsync<InventoryItem>(sql, new { id });
                if (deletedInventoryItem == null)
                    throw new Exception("InventoryItem not deleted");
                return deletedInventoryItem;
            }
        }
    }
}