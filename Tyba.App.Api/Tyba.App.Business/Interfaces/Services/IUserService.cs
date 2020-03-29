using System.Threading.Tasks;
using Tyba.App.Business.Models;

namespace Tyba.App.Business.Interfaces.Services
{
    public interface IUserService
    {
        Task<User> GetUserByEmail(string email);
    }
}
