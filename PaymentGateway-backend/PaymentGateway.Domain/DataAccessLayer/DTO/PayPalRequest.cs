using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Domain.DataAccessLayer.DTO
{
    public class PayPalRequest
    {
        public string OrderId { get; set; } = null!;
        public int loginUserId { get; set; }
        public decimal ExpectedAmount { get; set; }
        public string ExpectedCurrency { get; set; } = null!;
    }
}
