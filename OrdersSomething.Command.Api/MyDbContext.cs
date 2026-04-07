using Microsoft.EntityFrameworkCore;
using OrdersSomething.Command.Api.Models;

namespace OrdersSomething.Command.Api;

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

        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        var userId1 = new Guid("99999999-9999-9999-9999-999999999991");
        var userId2 = new Guid("99999999-9999-9999-9999-999999999992");

        var propId1 = new Guid("11111111-1111-1111-1111-111111111111");
        var propId2 = new Guid("22222222-2222-2222-2222-222222222222");
        var propId3 = new Guid("33333333-3333-3333-3333-333333333333");
        var propId4 = new Guid("44444444-4444-4444-4444-444444444444");

        var devId1 = new Guid("A1111111-1111-1111-1111-111111111111");
        var devId2 = new Guid("A2222222-2222-2222-2222-222222222222");
        var devId3 = new Guid("A3333333-3333-3333-3333-333333333333");
        var devId4 = new Guid("A4444444-4444-4444-4444-444444444444");

        var eventId1 = new Guid("E1111111-1111-1111-1111-111111111111");
        var eventId2 = new Guid("E2222222-2222-2222-2222-222222222222");

        var notifyId1 = new Guid("D1111111-1111-1111-1111-111111111111");
        var notifyId2 = new Guid("D2222222-2222-2222-2222-222222222222");

        var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<User>().HasData(
            new User { Id = userId1, Firstname = "John", Surname = "Doe" },
            new User { Id = userId2, Firstname = "Jane", Surname = "Smith" }
        );

        modelBuilder.Entity<Properties>().HasData(
            new Properties { Id = propId1, UserId = userId1, Name = "Dom Letniskowy", Address = "Mazury 1", Description = "Domek nad jeziorem", IsDeleted = false, CreatedAt = seedDate },
            new Properties { Id = propId2, UserId = userId1, Name = "Mieszkanie Kraków", Address = "Rynek 15", Description = "Apartament w centrum", IsDeleted = false, CreatedAt = seedDate },
            new Properties { Id = propId3, UserId = userId1, Name = "Działka Rekreacyjna", Address = "Bieszczady 10", Description = "Teren pod lasem", IsDeleted = false, CreatedAt = seedDate },
            new Properties { Id = propId4, UserId = userId2, Name = "Biuro Warszawa", Address = "Al. Jerozolimskie 100", Description = "Główne biuro", IsDeleted = false, CreatedAt = seedDate }
        );

        modelBuilder.Entity<Devices>().HasData(
            new Devices { Id = devId1, PropertiesId = propId1, Name = "Kamera Wejście", Type = "camera", Status = "active", IsListening = true, IsDeleted = false, LastHeartbeat = seedDate, CreatedAt = seedDate },
            new Devices { Id = devId2, PropertiesId = propId1, Name = "Czujnik Ruchu Salon", Type = "sensor", Status = "active", IsListening = true, IsDeleted = false, LastHeartbeat = seedDate, CreatedAt = seedDate },
            new Devices { Id = devId3, PropertiesId = propId2, Name = "Mikrofon Sypialnia", Type = "microphone", Status = "inactive", IsListening = false, IsDeleted = false, LastHeartbeat = seedDate, CreatedAt = seedDate },
            new Devices { Id = devId4, PropertiesId = propId4, Name = "Kamera Recepcja", Type = "camera", Status = "active", IsListening = true, IsDeleted = false, LastHeartbeat = seedDate, CreatedAt = seedDate }
        );

        modelBuilder.Entity<DeviceEvents>().HasData(
            new DeviceEvents { Id = eventId1, DeviceId = devId1, EventType = "motion", Description = "Wykryto ruch pod drzwiami", Severity = 2, CreatedAt = seedDate },
            new DeviceEvents { Id = eventId2, DeviceId = devId1, EventType = "capture", Description = "Zapisano obraz twarzy", ImageUrl = "https://example.com/img1.jpg", Severity = 1, CreatedAt = seedDate }
        );

        modelBuilder.Entity<UserNotifications>().HasData(
            new UserNotifications { Id = notifyId1, UserId = userId1, EventId = eventId1, Title = "Ruch wykryty!", Body = "Ktoś jest pod Twoimi drzwiami.", IsRead = false, SentAt = seedDate },
            new UserNotifications { Id = notifyId2, UserId = userId1, EventId = eventId2, Title = "Zapisano obraz", Body = "Nowe zdjęcie z kamery wejściowej.", IsRead = true, SentAt = seedDate }
        );
    }
}