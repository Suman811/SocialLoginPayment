using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Domain.DataAccessLayer.DTO
{
    public class GoogleAuthDto
    {
        public string? googleID { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
    }
}
