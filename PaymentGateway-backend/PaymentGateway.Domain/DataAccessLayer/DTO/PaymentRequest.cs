using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Domain.DataAccessLayer.DTO
{
    public class PaymentRequest
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string SourceToken { get; set; }
        public long Amount { get; set; }
    }
}
