using Microsoft.AspNetCore.Mvc;
using Order.Models;
using Order.Services;
using SharedLibrary.Interfaces;

namespace Order.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IDBService<OrderItem> _orderService;

        public OrderController(IDBService<OrderItem> orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders(){
            var orders = await _orderService.GetAllAsync();
            if (!orders.Any())
                return NotFound();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id){
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
                return NotFound();
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderItem order){
            var orderId = (await _orderService.CreateAsync(order)).OrderId;
            order.OrderId = orderId;
            var actionName = nameof(GetOrderById);
            var routeValues = new { id = orderId };
            return CreatedAtAction(actionName, routeValues, order);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrder(OrderItem order){
            var updatedOrder = await _orderService.UpdateAsync(order);
            return Ok(updatedOrder);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id){
            var deletedOrder = await _orderService.DeleteAsync(id);
            return Ok(deletedOrder);
        }
    }
}