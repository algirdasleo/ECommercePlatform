using Microsoft.AspNetCore.Mvc;
using Payment.Models;
using Payment.Services;
using SharedLibrary.Interfaces;

namespace Payment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IDBService<PaymentItem> _paymentService;

        public PaymentController(IDBService<PaymentItem> paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPayments(){
            var payments = await _paymentService.GetAllAsync();
            if (!payments.Any())
                return NotFound();
            return Ok(payments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById(int id){
            var payment = await _paymentService.GetByIdAsync(id);
            if (payment == null)
                return NotFound();
            return Ok(payment);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment(PaymentItem payment){
            var paymentId = (await _paymentService.CreateAsync(payment)).PaymentId;
            payment.PaymentId = paymentId;
            var actionName = nameof(GetPaymentById);
            var routeValues = new { id = paymentId };
            return CreatedAtAction(actionName, routeValues, payment);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePayment(PaymentItem payment){
            var updatedPayment = await _paymentService.UpdateAsync(payment);
            return Ok(updatedPayment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id){
            var deletedPayment = await _paymentService.DeleteAsync(id);
            return Ok(deletedPayment);
        }
    }
}