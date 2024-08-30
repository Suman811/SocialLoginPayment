using PaymentGateway.Domain.DataAccessLayer.DTO;
using PaymentGateway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Service.IService
{
    public interface ITransactionService
    {


        Task StoreTransactionAsync(StripePaymentTransactionDto transactionDto);
        Task PayPalStoreTransactionAsync(PayPalDto transactionDto);
        Task<PayPalTransaction> GetPayPalTransactionByIdAsync(string transactionId);




    }
}
