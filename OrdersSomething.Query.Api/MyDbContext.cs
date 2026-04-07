using Microsoft.EntityFrameworkCore;
using OrdersSomething.Query.Api.Models;

namespace OrdersSomething.Query.Api;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Properties> Properties { get; set; }
    public DbSet<Devices> Devices { get; set; }
    public DbSet<DeviceEvents> DeviceEvents { get; set; }
    public DbSet<UserNotifications> UserNotifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Firstname).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Surname).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<Properties>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        });

        modelBuilder.Entity<Devices>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Type).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
            entity.Property(e => e.IsListening).HasDefaultValue(true);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.LastHeartbeat).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        });

        modelBuilder.Entity<DeviceEvents>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.EventType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.ImageUrl).HasMaxLength(512);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        });

        modelBuilder.Entity<UserNotifications>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
            entity.Property(e => e.IsRead).HasDefaultValue(false);
            entity.Property(e => e.SentAt).HasDefaultValueSql("GETUTCDATE()");
        });

        // Relationships
        modelBuilder.Entity<User>()
            .HasMany(u => u.Properties)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId);

        modelBuilder.Entity<Properties>()
            .HasMany(p => p.Devices)
            .WithOne(d => d.Properties)
            .HasForeignKey(d => d.PropertiesId);

        modelBuilder.Entity<Devices>()
            .HasMany(d => d.DeviceEvents)
            .WithOne(e => e.Device)
            .HasForeignKey(e => e.DeviceId);

        // TODO: I think I don't need it - That should be handled by deviceEvents
        modelBuilder.Entity<User>()
            .HasMany<UserNotifications>()
            .WithOne(n => n.User)
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<DeviceEvents>()
            .HasMany<UserNotifications>()
            .WithOne(n => n.Event)
            .HasForeignKey(n => n.EventId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}