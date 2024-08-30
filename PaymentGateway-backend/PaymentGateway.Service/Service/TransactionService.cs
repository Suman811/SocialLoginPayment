using PaymentGateway.Domain.DataAccessLayer.DTO;
using PaymentGateway.Models;
using PaymentGateway.Repository.IRepository;
using PaymentGateway.Repository.Repository;
using PaymentGateway.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Service.Service
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IStripeService _stripeService;

        public TransactionService(ITransactionRepository repository, IUserRepository userRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
        }

        public async Task StoreTransactionAsync(StripePaymentTransactionDto transactionDto)
        {
            
            // Convert DTO to entity
            var transaction = new StripeTransaction
            {
                TransactionId = transactionDto.TransactionId,
                UserId = transactionDto.UserId,
                Amount = transactionDto.Amount,
                Currency = transactionDto.Currency,
                PaymentMethodId = transactionDto.PaymentMethodId,
                PaymentMethodBrand = transactionDto.PaymentMethodBrand,
                Description = transactionDto.Description,
                Status = transactionDto.Status,
                ReceiptUrl = transactionDto.ReceiptUrl,
                CreatedDate = transactionDto.CreatedDate,
                Paid = transactionDto.Paid,
                BalanceTransactionId = transactionDto.BalanceTransactionId,
                CustomerId = transactionDto.CustomerId,
                PaymentIntentId = transactionDto.PaymentIntentId,
                SourceId = transactionDto.SourceId,
                ShippingAddress = transactionDto.ShippingAddress
            };

            // Add transaction to repository
            await _repository.AddTransactionAsync(transaction);
        }

        public async Task PayPalStoreTransactionAsync(PayPalDto transactionDto)
        {
            // Convert DTO to entity
            var paypaltransaction = new PayPalTransaction
            {
                TransactionId = transactionDto.TransactionId,
                UserId = transactionDto.UserId,
                Amount = transactionDto.Amount,
                Currency = transactionDto.Currency,
                Status = transactionDto.Status,
                Paid = transactionDto.Paid,
                PayerId = transactionDto.PayerId,
            };

            // Add transaction to repository
            await _repository.PayPalAddTransactionAsync(paypaltransaction);
        }

        public async Task<PayPalTransaction> GetPayPalTransactionByIdAsync(string transactionId)
        {
            return await _repository.GetPayPalTransactionByIdAsync(transactionId);
        }

       

        //public async Task<TransactionDetailsDto> GetTransactionDetailsByIdAsync(string transactionId)
        //{
        //    // Validate input
        //    if (string.IsNullOrWhiteSpace(transactionId))
        //    {
        //        throw new ArgumentException("Transaction ID cannot be null or empty.", nameof(transactionId));
        //    }

        //    // Check if the transaction ID is for a Stripe invoice
        //    bool isStripeInvoice = IsStripeInvoiceId(transactionId);

        //    if (isStripeInvoice)
        //    {
        //        // Fetch invoice details from Stripe
        //        var invoice = await _stripeService.RetrieveInvoiceAsync(transactionId);

        //        // Convert Stripe invoice details to TransactionDetailsDto
        //        var transactionDetails = new TransactionDetailsDto
        //        {
        //            TransactionId = invoice.Id,
        //            Amount = invoice.AmountDue / 100m, // Amount is in cents, convert to dollars
        //            Currency = invoice.Currency,
        //            PaymentMethodBrand = invoice.PaymentIntent?.PaymentMethod?.Card?.Brand, // You may need to fetch PaymentIntent for more details
        //            Status = invoice.Status
        //        };

        //        return transactionDetails;
        //    }
        //    else
        //    {
        //        // Fetch transaction details from repository if not a Stripe invoice
        //        var transactionDetails = await _repository.GetTransactionDetailsByIdAsync(transactionId);

        //        if (transactionDetails == null)
        //        {
        //            throw new KeyNotFoundException($"Transaction with ID {transactionId} not found.");
        //        }

        //        return transactionDetails;
        //    }
        //}

        //private bool IsStripeInvoiceId(string transactionId)
        //{
        //    // Add logic to determine if the transactionId is a Stripe invoice ID
        //    // This could be based on ID format, prefix, or another convention
        //    return transactionId.StartsWith("in_"); // Example prefix for Stripe invoice IDs
        //}

    }
}
