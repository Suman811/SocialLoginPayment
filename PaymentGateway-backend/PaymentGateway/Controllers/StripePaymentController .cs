using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Domain.DataAccessLayer.DTO;
using PaymentGateway.Service.IService;
using Stripe;

namespace PaymentGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripePaymentController : ControllerBase
    {
        private readonly IStripeService _stripeService;
        private readonly IConfiguration _configuration;
        private readonly ITransactionService _transactionService;

        public StripePaymentController(IStripeService stripeService, IConfiguration configuration, ITransactionService transactionService)
        {
            _stripeService = stripeService;
            _configuration = configuration;
            _transactionService = transactionService;
        }

        [HttpPost("create-payment")]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentRequest request)
        {
            // Ensure that the Stripe API key is set
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];

            var customerId = await _stripeService.CreateCustomerAsync(request.Email, request.SourceToken);
            var chargeId = await _stripeService.CreateChargeAsync(customerId, request.Amount, "Subscription Payment", "usd");

            var chargeService = new ChargeService();
            var charge = await chargeService.GetAsync(chargeId);

            if (charge.Status == "succeeded")
            {
                var transactionDto = new StripePaymentTransactionDto
                {
                    TransactionId = chargeId,
                    UserId = request.UserId,
                    Amount = charge.Amount / 100m, // Stripe amounts are in cents
                    Currency = charge.Currency,
                    PaymentMethodId = charge.PaymentMethod,
                    PaymentMethodBrand = charge.PaymentMethodDetails?.Card?.Brand,
                    Description = charge.Description,
                    Status = charge.Status,
                    ReceiptUrl = charge.ReceiptUrl,
                    CreatedDate = charge.Created,
                    Paid = charge.Paid,
                    BalanceTransactionId = charge.BalanceTransactionId,
                    CustomerId = charge.CustomerId,
                    PaymentIntentId = charge.PaymentIntentId,
                    SourceId = charge.Id,
                    ShippingAddress = charge.Shipping?.Address?.Line1 // Adjust this as necessary
                };

                await _transactionService.StoreTransactionAsync(transactionDto);

                // Return success response
                return Ok("Payment was successful.");
            }
            else
            {
                // Save failed transaction data if needed
                var transactionDto = new StripePaymentTransactionDto
                {
                    TransactionId = chargeId,
                    UserId = request.UserId,
                    Amount = charge.Amount / 100m,
                    Currency = charge.Currency,
                    PaymentMethodId = charge.PaymentMethod,
                    PaymentMethodBrand = charge.PaymentMethodDetails?.Card?.Brand,
                    Description = charge.Description,
                    Status = charge.Status,
                    ReceiptUrl = charge.ReceiptUrl,
                    CreatedDate = charge.Created,
                    Paid = charge.Paid,
                    BalanceTransactionId = charge.BalanceTransactionId,
                    CustomerId = charge.CustomerId,
                    PaymentIntentId = charge.PaymentIntentId,
                    SourceId = charge.Id,
                    ShippingAddress = charge.Shipping?.Address?.Line1
                };

                await _transactionService.StoreTransactionAsync(transactionDto);

                // Return failure response
                return BadRequest(new { ChargeId = chargeId, Status = charge.Status });
            }
        }

          
        }
    }
    //public class StripePaymentController : ControllerBase
    //{
    //    private readonly IStripeService _stripeService;
    //    private readonly IConfiguration _configuration;
    //    private readonly ITransactionService _transactionService;

    //    public StripePaymentController(IStripeService stripeService, IConfiguration configuration, ITransactionService transactionService)
    //    {
    //        _stripeService = stripeService;
    //        _configuration = configuration;
    //        _transactionService = transactionService;
    //    }

    //    [HttpPost("create-payment")]
    //    public async Task<IActionResult> CreatePayment([FromBody] PaymentRequest request)
    //    {
    //        // Ensure that the Stripe API key is set
    //        StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];

    //        var customerId = await _stripeService.CreateCustomerAsync(request.Email, request.SourceToken);
    //        var chargeId = await _stripeService.CreateChargeAsync(customerId, request.Amount, "Subscription Payment", "usd");

    //        var chargeService = new ChargeService();
    //        var charge = await chargeService.GetAsync(chargeId);

    //        if (charge.Status == "succeeded")
    //        {
    //            var transactionDto = new StripePaymentTransactionDto
    //            {
    //                TransactionId = chargeId,
    //                UserId = request.UserId,
    //                Amount = charge.Amount / 100m, // Stripe amounts are in cents
    //                Currency = charge.Currency,
    //                PaymentMethodId = charge.PaymentMethod,
    //                PaymentMethodBrand = charge.PaymentMethodDetails?.Card?.Brand,
    //                Description = charge.Description,
    //                Status = charge.Status,
    //                ReceiptUrl = charge.ReceiptUrl,
    //                CreatedDate = charge.Created,
    //                Paid = charge.Paid,
    //                BalanceTransactionId = charge.BalanceTransactionId,
    //                CustomerId = charge.CustomerId,
    //                PaymentIntentId = charge.PaymentIntentId,
    //                SourceId = charge.Id,
    //                ShippingAddress = charge.Shipping?.Address?.Line1 // Adjust this as necessary
    //            };

    //            await _transactionService.StoreTransactionAsync(transactionDto);

    //            // Return success response
    //            return Ok("Payment was successful.");
    //        }
    //        else
    //        {
    //            // Save failed transaction data if needed
    //            var transactionDto = new StripePaymentTransactionDto
    //            {
    //                TransactionId = chargeId,
    //                UserId = request.UserId,
    //                Amount = charge.Amount / 100m,
    //                Currency = charge.Currency,
    //                PaymentMethodId = charge.PaymentMethod,
    //                PaymentMethodBrand = charge.PaymentMethodDetails?.Card?.Brand,
    //                Description = charge.Description,
    //                Status = charge.Status,
    //                ReceiptUrl = charge.ReceiptUrl,
    //                CreatedDate = charge.Created,
    //                Paid = charge.Paid,
    //                BalanceTransactionId = charge.BalanceTransactionId,
    //                CustomerId = charge.CustomerId,
    //                PaymentIntentId = charge.PaymentIntentId,
    //                SourceId = charge.Id,
    //                ShippingAddress = charge.Shipping?.Address?.Line1
    //            };

    //            await _transactionService.StoreTransactionAsync(transactionDto);

    //            // Return failure response
    //            return BadRequest(new { ChargeId = chargeId, Status = charge.Status });
    //        }
    //    }

    //    [HttpGet("transaction/{transactionId}")]
    //    public async Task<IActionResult> GetTransactionDetails(string transactionId)
    //    {
    //        if (string.IsNullOrWhiteSpace(transactionId))
    //        {
    //            return BadRequest(new { Message = "Transaction ID cannot be null or empty." });
    //        }

    //        try
    //        {
    //            // Check if the transaction ID is for a Stripe charge
    //            var chargeService = new ChargeService();
    //            var charge = await chargeService.GetAsync(transactionId);

    //            if (charge == null)
    //            {
    //                return NotFound(new { Message = "Charge not found." });
    //            }

    //            // Get transaction details from the service
    //            var transactionDetails = await _transactionService.GetTransactionDetailsByIdAsync(charge.Id);

    //            if (transactionDetails == null)
    //            {
    //                return NotFound(new { Message = "Transaction details not found." });
    //            }

    //            return Ok(transactionDetails);
    //        }
    //        catch (StripeException ex)
    //        {
    //            // Handle Stripe-specific exceptions
    //            return StatusCode((int)ex.HttpStatusCode, new { Message = ex.Message });
    //        }
    //        catch (Exception ex)
    //        {
    //            // Handle other exceptions
    //            return StatusCode(500, new { Message = "An error occurred while processing your request.", Details = ex.Message });
    //        }
    //    }

 