using PaymentGateway.Domain.DataAccessLayer.DTO;

using PaymentGateway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Repository.IRepository
{
    public interface IUserRepository
    {
        public Task<string> AddUser(PUser pUser);
        Task<bool> UserExistsByGoogleIdAsync(string googleId);
        Task<bool> UserExistsByFacebookIdAsync(string facebookId);
        Task<int?> FindUserIdByGoogleIdOrFacebookIdAsync(string googleId, string facebookId);

    }
}
