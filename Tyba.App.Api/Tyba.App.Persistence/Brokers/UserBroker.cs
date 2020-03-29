using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tyba.App.Persistence.Contexts;
using Tyba.App.Persistence.Entities;
using Tyba.App.Persistence.Interfaces.Brokers;

namespace Tyba.App.Persistence.Brokers
{
    public class UserBroker : IUserBroker
    {
        private readonly TybaDbContext _dbContext;

        public UserBroker(TybaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserEntity> GetUserByEmail(string email)
        {
            var result = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
            return result;
        }
    }
}
