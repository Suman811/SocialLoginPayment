using PaymentGateway.Domain.DataAccessLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Service.IService
{
    public interface IUserService
    {
        
        public Task<string> AddUser(GoogleAuthDto googleAuthDto);
        public Task<string>AddUser(FbAuthDto fbAuthDto);
    }
}
