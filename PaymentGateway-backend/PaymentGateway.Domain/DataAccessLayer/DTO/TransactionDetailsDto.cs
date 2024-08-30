using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Domain.DataAccessLayer.DTO
{
    public class TransactionDetailsDto
    {
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string PaymentMethodBrand { get; set; }
        public string Status { get; set; }

    }
}
