using System.Threading.Tasks;
using Tyba.App.Business.Models;
using Tyba.App.Business.Models.Dtos;

namespace Tyba.App.Business.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserRegistrationResponse> RegisterUser(User newUser);

        Task<User> Authenticate(User userCredentials);

        Task<User> GetUserByEmail(string email);
    }
}
