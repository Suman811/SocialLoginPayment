using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Domain.DataAccessLayer.DTO
{
    public class StripeInvoiceDto
    {
        public string InvoiceId { get; set; }
        public string CustomerId { get; set; }
        public string HostedInvoiceUrl { get; set; }
        public long Amount { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
