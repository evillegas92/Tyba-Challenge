using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tyba.App.Persistence.Entities;

namespace Tyba.App.Persistence.Interfaces.Brokers
{
    public interface IUserBroker
    {
        Task<UserEntity> GetUserByEmail(string email);
    }
}
