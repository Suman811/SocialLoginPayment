using PaymentGateway.Domain.DataAccessLayer.DTO;
using PaymentGateway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Repository.IRepository
{
    public interface ITransactionRepository
    {


        Task AddTransactionAsync(StripeTransaction transaction);

        Task PayPalAddTransactionAsync(PayPalTransaction transaction);

        Task<PayPalTransaction> GetPayPalTransactionByIdAsync(string transactionId);

        Task<TransactionDetailsDto> GetTransactionDetailsByIdAsync(string transactionId);


    }
}
