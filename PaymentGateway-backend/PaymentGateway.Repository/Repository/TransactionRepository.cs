using Microsoft.EntityFrameworkCore;
using PaymentGateway.Domain.DataAccessLayer.DTO;
using PaymentGateway.Models;
using PaymentGateway.Repository.IRepository;

namespace PaymentGateway.Repository.Repository
{

    public class TransactionRepository : ITransactionRepository
    {
        private readonly SDirectContext _context;

        public TransactionRepository(SDirectContext context)
        {
            _context = context;
        }

        public async Task AddTransactionAsync(StripeTransaction transaction)
        {
            await _context.StripeTransactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }
        public async Task PayPalAddTransactionAsync(PayPalTransaction transaction)
        {
            await _context.PayPalTransactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task<PayPalTransaction> GetPayPalTransactionByIdAsync(string transactionId)
        {
            return await _context.PayPalTransactions
                .FirstOrDefaultAsync(t => t.TransactionId == transactionId);
        }

        public async Task<TransactionDetailsDto> GetTransactionDetailsByIdAsync(string transactionId)
        {
            var transaction = await _context.StripeTransactions
                .Where(t => t.TransactionId == transactionId)
                .Select(t => new TransactionDetailsDto
                {
                    TransactionId = t.TransactionId,
                    Amount = t.Amount,
                    Currency = t.Currency,
                    PaymentMethodBrand = t.PaymentMethodBrand,
                    Status = t.Status
                })
                .FirstOrDefaultAsync();

            return transaction;
        }




    }
}

