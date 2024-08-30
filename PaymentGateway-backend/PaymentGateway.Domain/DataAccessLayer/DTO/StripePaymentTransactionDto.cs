using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Domain.DataAccessLayer.DTO
{
    public class StripePaymentTransactionDto
    {
        public string TransactionId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string PaymentMethodId { get; set; }
        public string PaymentMethodBrand { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string ReceiptUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Paid { get; set; }
        public string BalanceTransactionId { get; set; }
        public string CustomerId { get; set; }
        public string PaymentIntentId { get; set; }
        public string SourceId { get; set; }
        public string ShippingAddress { get; set; }
    }
}
