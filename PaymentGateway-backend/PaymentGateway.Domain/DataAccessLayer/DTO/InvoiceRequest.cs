using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Domain.DataAccessLayer.DTO
{
    public class InvoiceRequest
    {
        public string CustomerId { get; set; }
        public int Amount { get; set; } // Amount in dollars
        public string Description { get; set; }

    }
}
