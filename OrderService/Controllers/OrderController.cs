using Microsoft.AspNetCore.Mvc;
using OrderService.Models;
using OrderService.Services;
using SharedLibrary.Interfaces;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IDBService<Order> _orderService;

        public OrderController(IDBService<Order> orderService)
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
        public async Task<IActionResult> CreateOrder(Order order){
            var orderId = (await _orderService.CreateAsync(order)).OrderId;
            order.OrderId = orderId;
            var actionName = nameof(GetOrderById);
            var routeValues = new { id = orderId };
            return CreatedAtAction(actionName, routeValues, order);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrder(Order order){
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