using Microsoft.EntityFrameworkCore;

namespace Alarmist.Infrastructure.Persistence.Data;

public class AlarmistContext(DbContextOptions<AlarmistContext> options) : DbContext(options)
{
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Tutaj będziemy dodawać konfiguracje dla naszych encji
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AlarmistContext).Assembly);
    }
} 