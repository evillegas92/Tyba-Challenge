using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Tyba.App.Business.Interfaces.Services;
using Tyba.App.Business.Models;
using Tyba.App.Business.Models.Dtos;
using Tyba.App.Persistence.Entities;
using Tyba.App.Persistence.Interfaces.Brokers;

namespace Tyba.App.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserBroker _userBroker;
        private readonly IMapper _autoMapper;

        public UserService(IUserBroker userBroker, IMapper autoMapper)
        {
            _userBroker = userBroker;
            _autoMapper = autoMapper;
        }

        public async Task<UserRegistrationResponse> RegisterUser(User newUser)
        {
            User existingUser = await GetUserByEmail(newUser.Email);
            if (existingUser != null)
                return new UserRegistrationResponse
                {
                    Success = false,
                    ErrorMessage = "Email already exists."
                };
            string hashedPassword = GenerateMd5Hash(newUser.Password);
            newUser.Password = hashedPassword;
            UserEntity newUserEntity = _autoMapper.Map<UserEntity>(newUser);

            int rowsAffected = await _userBroker.AddUser(newUserEntity);
            if (rowsAffected <= 0)
                throw new System.Exception("User could not be registered.");
            
            return new UserRegistrationResponse { Success = true };
        }

        public async Task<User> Authenticate(User user)
        {
            User existingUser = await GetUserByEmail(user.Email);
            if (existingUser == null)
                return null;
            string providedPasswordHash = GenerateMd5Hash(user.Password);
            if (providedPasswordHash != existingUser.Password)
                return null;

            return existingUser;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            UserEntity userEntity = await _userBroker.GetUserByEmail(email);
            User user = _autoMapper.Map<User>(userEntity);
            return user;
        }
        
        private static string GenerateMd5Hash(string inputString)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(inputString));

                // Create a new Stringbuilder to collect the bytes and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                return sBuilder.ToString();
            }
        }
    }
}
