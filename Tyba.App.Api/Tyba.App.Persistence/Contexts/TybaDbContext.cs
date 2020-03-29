using Microsoft.EntityFrameworkCore;
using Tyba.App.Persistence.Entities;

namespace Tyba.App.Persistence.Contexts
{
    public class TybaDbContext : DbContext
    {
        public TybaDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<UserEntity> Users {get ; set;}
    }
}
