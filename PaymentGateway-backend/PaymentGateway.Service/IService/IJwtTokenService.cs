using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Service.IService
{
    public interface IJwtTokenService
    {
     string GenerateToken(string userId);

    }
}
