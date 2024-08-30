using Microsoft.EntityFrameworkCore;
using PaymentGateway.Domain.DataAccessLayer.DTO;

using PaymentGateway.Models;
using PaymentGateway.Repository.IRepository;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Repository.Repository
{
    public class UserRepository:IUserRepository
    { 
        private readonly SDirectContext _context;
        
            public UserRepository(SDirectContext context) 
        {
            _context = context;
        }

        public async Task<string> AddUser(PUser pUser)
        {
            _context.PUsers.Add(pUser);
            await _context.SaveChangesAsync();
            return "User logged in and added succesfully";
         
        }
        public async Task<bool> UserExistsByGoogleIdAsync(string googleId)
        {
            return await _context.PUsers.AnyAsync(u => u.GoogleId == googleId);
        }
        public async Task<bool> UserExistsByFacebookIdAsync(string facebookId)
        {
            return await _context.PUsers.AnyAsync(u => u.FacebookId == facebookId);
        }

        public async Task<int?> FindUserIdByGoogleIdOrFacebookIdAsync(string googleId, string facebookId)
        {
            var user = await _context.PUsers
                .Where(u => u.GoogleId == googleId || u.FacebookId == facebookId)
                .Select(u => u.UserId)
                .FirstOrDefaultAsync();

            return user;
        }
    }
}
