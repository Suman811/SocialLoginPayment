using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Service.IService
{
    public interface IStripeService
    {
        Task<string> CreateCustomerAsync(string email, string sourceToken);
        Task<string> CreateChargeAsync(string customerId, long amount, string description, string currency);
        // Method to create an invoice item

        // Method to create an invoice
        //Task<Invoice> CreateInvoiceAsync(string customerId);
        //Task<Invoice> FinalizeInvoiceAsync(string invoiceId);
        //Task<Invoice> RetrieveInvoiceAsync(string invoiceId);
    }
}
