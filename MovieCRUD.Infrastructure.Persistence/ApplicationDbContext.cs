using Microsoft.AspNet.Identity.EntityFramework;
using MovieCRUD.Infrastructure.Persistence.Entities;
using System.Data.Entity;

namespace MovieCRUD.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<MovieEntity> Movies { get; set; }

        public ApplicationDbContext() : base("ApplicationDb")
        {
            var ensureDllIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }
    }
}