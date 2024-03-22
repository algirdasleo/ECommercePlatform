using Microsoft.AspNetCore.Mvc;
using Inventory.Models;
using Inventory.Services;
using SharedLibrary.Interfaces;

namespace Inventory.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IDBService<InventoryItem> _inventoryService;

        public InventoryController(IDBService<InventoryItem> inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInventoryItems(){
            var inventoryItems = await _inventoryService.GetAllAsync();
            if (!inventoryItems.Any())
                return NotFound();
            return Ok(inventoryItems);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInventoryItemById(int id){
            var inventoryItem = await _inventoryService.GetByIdAsync(id);
            if (inventoryItem == null)
                return NotFound();
            return Ok(inventoryItem);
        }

        [HttpPost]
        public async Task<IActionResult> CreateInventoryItem(InventoryItem inventoryItem){
            var inventoryItemId = (await _inventoryService.CreateAsync(inventoryItem)).InventoryItemId;
            inventoryItem.InventoryItemId = inventoryItemId;
            var actionName = nameof(GetInventoryItemById);
            var routeValues = new { id = inventoryItemId };
            return CreatedAtAction(actionName, routeValues, inventoryItem);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateInventoryItem(InventoryItem inventoryItem){
            var updatedInventoryItem = await _inventoryService.UpdateAsync(inventoryItem);
            return Ok(updatedInventoryItem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventoryItem(int id){
            var deletedInventoryItem = await _inventoryService.DeleteAsync(id);
            return Ok(deletedInventoryItem);
        }
    }
}