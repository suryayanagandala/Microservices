using Identity.CoreModels.Entities;
using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.CoreModels.Persistance
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }

        public virtual DbSet<Roles> Roles=> Set<Roles>();
        
        public virtual DbSet<Users> Users {  get; set; }

        public virtual DbSet<RefreshTokens> RefreshTokens => Set<RefreshTokens>();

        public virtual DbSet<OtpManager> OtpManagers => Set<OtpManager>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
