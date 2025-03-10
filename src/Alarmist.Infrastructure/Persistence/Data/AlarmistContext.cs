using Alarmist.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Alarmist.Infrastructure.Persistence.Data;

public class AlarmistContext(DbContextOptions<AlarmistContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AlarmistContext).Assembly);
    }
} 