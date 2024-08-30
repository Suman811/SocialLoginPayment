using PaymentGateway.Service.IService;
using Stripe;
using Stripe.FinancialConnections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PaymentGateway.Service.Service
{
    public class StripeService : IStripeService
    {
        private readonly string _apiKey;

        public StripeService(string apiKey)
        {
            _apiKey = apiKey;
            StripeConfiguration.ApiKey = _apiKey;
        }

        public async Task<string> CreateCustomerAsync(string email, string sourceToken)
        {
            var customers = new CustomerService();
            var customer = await customers.CreateAsync(new CustomerCreateOptions
            {
                Email = email,
                Source = "tok_visa"
            });
            return customer.Id;
        }

        public async Task<string> CreateChargeAsync(string customerId, long amount, string description, string currency)
        {
            var charges = new ChargeService();
            var charge = await charges.CreateAsync(new ChargeCreateOptions
            {
                Amount = amount,
                Description = description,
                Currency = currency,
                Customer = customerId
            });
            return charge.Id;
        }

        //public async Task<string> CreateInvoiceAsync(string customerId)
        //{
        //    var invoices = new InvoiceService();
        //    var invoice = await invoices.CreateAsync(new InvoiceCreateOptions
        //    {
        //        Customer = customerId
        //    });
        //    return invoice.Id;
        //}

        //public async Task<string> FinalizeInvoiceAsync(string invoiceId)
        //{
        //    var invoices = new InvoiceService();
        //    var invoice = await invoices.FinalizeInvoiceAsync(invoiceId);
        //    return invoice.Id;
        //}

        //public async Task<Invoice> RetrieveInvoiceAsync(string invoiceId)
        //{
        //    var invoices = new InvoiceService();
        //    var invoice = await invoices.GetAsync(invoiceId);
        //    return invoice;
        //}
    }


}


