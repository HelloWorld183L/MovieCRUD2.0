using MovieCRUD.Domain.Authentication;
using MovieCRUD.Infrastructure.Persistence.Entities;
using System;
using System.Data.Entity;

namespace MovieCRUD.Infrastructure
{
    public class ApplicationDbContext : DbContext, IDisposable
    {
        public DbSet<MovieEntity> Movies { get; set; }
        public DbSet<UserLoginData> LoginInfo { get; set; }


        public ApplicationDbContext() : base("ApplicationDb")
        {
            var ensureDllIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }
    }
}