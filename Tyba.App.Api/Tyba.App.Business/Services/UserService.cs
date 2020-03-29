using System.Threading.Tasks;
using AutoMapper;
using Tyba.App.Business.Interfaces.Services;
using Tyba.App.Business.Models;
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

        public async Task<User> GetUserByEmail(string email)
        {
            UserEntity userEntity = await _userBroker.GetUserByEmail(email);
            User user = _autoMapper.Map<User>(userEntity);
            return user;
        }
    }
}
