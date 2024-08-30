using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Domain.DataAccessLayer.DTO;
using PaymentGateway.Service.IService;
using PayPalCheckoutSdk.Orders;


namespace PaymentGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayPalController : ControllerBase
    {

        private readonly IPayPalService _payPalService;
        private readonly ITransactionService _transactionService;

        public PayPalController(IPayPalService payPalService, ITransactionService transactionService)
        {
            _payPalService = payPalService;
            _transactionService = transactionService;
        }

        [HttpPost("process-request")]
        public async Task<IActionResult> ProcessRequest([FromBody] PayPalRequest request)
        {
            if (string.IsNullOrEmpty(request.OrderId))
            {
                return BadRequest("Order ID is required.");
            }

            // Verify the transaction first
            if (request.ExpectedAmount <= 0 || string.IsNullOrEmpty(request.ExpectedCurrency))
            {
                return BadRequest("Invalid verification request data.");
            }

            var isVerified = await _payPalService.VerifyTransactionAsync(request.OrderId, request.ExpectedAmount, request.ExpectedCurrency);
            if (!isVerified)
            {
                return BadRequest("Transaction verification failed.");
            }

            // Get order details if verification is successful
            var order = await _payPalService.GetOrderDetailsAsync(request.OrderId);
            if (order == null)
            {
                return NotFound("Order not found.");
            }

            // Convert order details to DTO and save to database
            var transactionDto = new PayPalDto
            {
                TransactionId = order.Id,
                UserId = request.loginUserId, // Implement this method based on your logic
                Amount = decimal.Parse(order.PurchaseUnits[0].AmountWithBreakdown.Value),
                Currency = order.PurchaseUnits[0].AmountWithBreakdown.CurrencyCode,
                Status = order.Status,
                Paid = order.Status == "COMPLETED",
                PayerId = order.Payer?.PayerId,
            };

            await _transactionService.PayPalStoreTransactionAsync(transactionDto);

            return Ok("Transaction processed and stored successfully.");
        }

        [HttpGet("invoice/{transactionId}")]
        public async Task<IActionResult> GetTransactionInvoice(string transactionId)
        {
            var transaction = await _transactionService.GetPayPalTransactionByIdAsync(transactionId);
            if (transaction == null)
            {
                return NotFound("Transaction not found.");
            }

            var transactionDetails = new
            {
                transaction.TransactionId,
                transaction.Amount,
                transaction.Currency,
                transaction.Status,
                TimeOfTransaction = transaction.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss")
            };


            return Ok(transactionDetails);
        }

        private int GetUserIdFromOrder(Order order)
        {
            // Implement logic to extract or determine UserId from the order
            // For example, this might be part of your application logic or could be passed in the request
            return 0; // Placeholder
        }

    }
}
    

