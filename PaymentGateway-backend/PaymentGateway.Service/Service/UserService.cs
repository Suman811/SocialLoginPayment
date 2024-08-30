using Microsoft.EntityFrameworkCore;
using PaymentGateway.Domain.DataAccessLayer.DTO;

using PaymentGateway.Models;
using PaymentGateway.Repository.IRepository;
using PaymentGateway.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Service.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<string> AddUser(GoogleAuthDto googleAuthDto)
        {

            // Check if user with the given Google ID already exists
            if (await _userRepository.UserExistsByGoogleIdAsync(googleAuthDto.googleID))
            {
                return "User already exists";
            }



            // User does not exist, so add them



            PUser pUser = new PUser();
                pUser.GoogleId = googleAuthDto.googleID;
                pUser.Email = googleAuthDto.Email;
                pUser.Name = googleAuthDto.UserName;
            
            return await _userRepository.AddUser(pUser);
            


                //return "User logged in and registered successfully";
            
           
           
           
            
            
        }

        public async Task<string> AddUser(FbAuthDto fbAuthDto)
        {
            if (await _userRepository.UserExistsByFacebookIdAsync(fbAuthDto.FacebookID))
            {
                return "User already exists";
            }


            PUser pUser = new PUser
            {
                FacebookId = fbAuthDto.FacebookID,
                Email = fbAuthDto.Email,
                Name = fbAuthDto.UserName
            };

            return await _userRepository.AddUser(pUser);
        }
    }

}
