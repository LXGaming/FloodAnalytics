using LXGaming.FloodAnalytics.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LXGaming.FloodAnalytics.Storage; 

public class StorageContext : DbContext {

    private static readonly ValueConverter<DateTime, DateTime> DateTimeConverter = new(
        value => value,
        value => DateTime.SpecifyKind(value, DateTimeKind.Local));
    
    public DbSet<Torrent> Torrents { get; set; } = null!;
    public DbSet<Traffic> Traffic { get; set; } = null!;

    public StorageContext(DbContextOptions options) : base(options) {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        ApplyDateTimeConverter(modelBuilder);
        
        // Fix columns
        modelBuilder.Entity<Torrent>().Property(model => model.Trackers).HasConversion(
            value => string.Join(',', value),
            value => value.Split(',', StringSplitOptions.RemoveEmptyEntries),
            new ArrayStructuralComparer<string>());
        modelBuilder.Entity<Traffic>().Property(model => model.PercentComplete).HasColumnType("decimal(6,3)");
        modelBuilder.Entity<Traffic>().Property(model => model.Ratio).HasColumnType("decimal(6,3)");
    }

    public override int SaveChanges() {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps() {
        var now = DateTime.Now;
        var entities = ChangeTracker.Entries();
        foreach (var entity in entities) {
            var createdAt = entity.Properties.SingleOrDefault(entry => entry.Metadata.Name.Equals("CreatedAt"));
            if (createdAt != null && entity.State == EntityState.Added) {
                if (createdAt.CurrentValue == null || createdAt.CurrentValue.Equals(default(DateTime))) {
                    createdAt.CurrentValue = now;
                }
            }

            var updatedAt = entity.Properties.SingleOrDefault(entry => entry.Metadata.Name.Equals("UpdatedAt"));
            if (updatedAt != null && entity.State is EntityState.Added or EntityState.Modified) {
                updatedAt.CurrentValue = now;
            }
        }
    }
    
    private void ApplyDateTimeConverter(ModelBuilder modelBuilder) {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes()) {
            foreach (var property in entityType.GetProperties()) {
                if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?)) {
                    property.SetValueConverter(DateTimeConverter);
                }
            }
        }
    }
}