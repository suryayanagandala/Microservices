using Cart.CoreModels.Entities;
using Cart.Infrastructure.Inbox;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.Infrastructure
{
    public class CartDbContext:DbContext
    {
        public CartDbContext(DbContextOptions<CartDbContext> options):base(options)
        {
            
        }

        public virtual DbSet<Carts> Cart { get; set; }

        public virtual DbSet<CartItem> CartItems { get; set; }

        public DbSet<InboxMessage> InboxMessages => Set<InboxMessage>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CartDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}

