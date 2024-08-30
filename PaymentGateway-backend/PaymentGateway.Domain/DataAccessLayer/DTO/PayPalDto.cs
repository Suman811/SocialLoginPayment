using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Domain.DataAccessLayer.DTO
{
    public class PayPalDto
    {
        public string TransactionId { get; set; } = null!;
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = null!;
        public string Status { get; set; } = null!;
        public bool Paid { get; set; }
        public string? PayerId { get; set; }
    }
}
