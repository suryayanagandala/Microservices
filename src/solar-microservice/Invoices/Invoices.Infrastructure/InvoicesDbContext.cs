using Invoices.CoreModels.Entities;
using Microsoft.EntityFrameworkCore;

namespace Invoices.Infrastructure;

public sealed class InvoicesDbContext(DbContextOptions<InvoicesDbContext> options)
    : DbContext(options)
{
    public DbSet<Invoice> Invoices => Set<Invoice>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InvoicesDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
