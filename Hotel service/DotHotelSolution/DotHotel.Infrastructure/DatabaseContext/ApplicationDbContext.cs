using System.Text.Json;
using System.Text.Json.Serialization;
using DotHotel.Core.Domain.InfrastructureContracts.Identity;
using DotHotel.Core.Domain.Models.HotelModels;
using DotHotel.Core.Domain.Models.IdentityModels;
using DotHotel.Core.Domain.Models.TrackerModels;
using DotHotel.Core.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;

namespace DotHotel.Infrastructure.DatabaseContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        private readonly ICurrentUserService? _currentUserService;

        [ActivatorUtilitiesConstructor]
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService currentUserService) : base(options)
        {
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }

        public virtual DbSet<Apartment> Apartments { get; set; } = null!;
        public virtual DbSet<ApartmentImage> ApartmentImages { get; set; } = null!;
        public virtual DbSet<Reservation> Reservations { get; set; } = null!;
        public virtual DbSet<EntityModificationHistory> EntityModificationHistories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Reservation entity configuration

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.ReservationHolder)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.ReservationHolderId);

            modelBuilder.Entity<Reservation>().HasQueryFilter(r =>
                _currentUserService == null ||
                _currentUserService.UserRole == Core.Enums.UserRoles.Administrator.ToString() ||
                r.ReservationHolderId.ToString() == _currentUserService.UserId);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Apartment)
                .WithMany(a => a.Reservations)
                .HasForeignKey(r => r.ApartmentId);

            #endregion

            #region Apartment entity configuration

            modelBuilder.Entity<Apartment>()
                .HasMany(a => a.ApartmentImages)
                .WithOne(i => i.Apartment)
                .HasForeignKey(r => r.ApartmentId);

            #endregion
        }

        public override int SaveChanges()
        {
            TrackEntityModifications();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            TrackEntityModifications();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void TrackEntityModifications()
        {
            var now = DateTime.UtcNow;
            var userId = _currentUserService?.UserId ?? "system";

            var auditEntries = new List<EntityModificationHistory>();

            foreach (var entry in ChangeTracker.Entries().Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted))
            {
                if (entry.Entity is EntityModificationHistory) continue;

                var entityId = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey())?.CurrentValue?.ToString() ?? "unknown";
                var entityName = entry.Entity.GetType().Name;

                var currentState = new Dictionary<string, object?>();

                foreach (var prop in entry.Properties)
                {
                    currentState[prop.Metadata.Name] = entry.State == EntityState.Deleted ? prop.OriginalValue : prop.CurrentValue;
                }

                var audit = new EntityModificationHistory
                {
                    Id = Guid.NewGuid(),
                    EntityName = entityName,
                    EntityId = entityId,
                    Action = entry.State.ToString(),
                    ChangedByUserId = userId,
                    ChangedAt = now,
                    ChangesJson = JsonSerializer.Serialize(
                        currentState,
                        new JsonSerializerOptions
                        {
                            WriteIndented = false,
                            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                        })
                };

                auditEntries.Add(audit);
            }

            if (auditEntries.Count > 0)
            {
                EntityModificationHistories.AddRange(auditEntries);
            }
        }
    }
}
