using AdminService.DataContext.Configurations;
using AdminService.Domain.Models.ComplaintAggregate;
using Microsoft.EntityFrameworkCore;

namespace AdminService.DataContext.Database;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Complaint> Complaints { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new ComplaintConfigurations());
        base.OnModelCreating(builder);
    }
}