using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RealEstateCRM.Core.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RealEstateCRM.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Represents the Leads table (Potential Clients)
        public DbSet<Lead> Leads => Set<Lead>();

        // Represents the InteractionLogs table (Call notes, meetings, and client history)
        public DbSet<InteractionLog> InteractionLogs => Set<InteractionLog>();

        // Represents the Properties table (Real Estate Units and Listings)
        public DbSet<Property> Properties => Set<Property>();

        // Represents the Deals table (Sales & Contracts)
        public DbSet<Deal> Deals => Set<Deal>();

        // Represents the Appointments table (Viewing Sessions)
        public DbSet<Appointment> Appointments => Set<Appointment>();
        public DbSet<Notification> Notifications => Set<Notification>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();


        /// <summary>
        /// OnModelCreating configures entity relationships, column types, precision, soft delete query filters, and constraints using Fluent API.
        /// </summary>
        /// <param name="builder">The ModelBuilder instance used for entity schema configurations.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Call the base method to ensure ASP.NET Core Identity tables are correctly configured
            base.OnModelCreating(builder);

            // 1. Configure Lead Entity Constraints
            builder.Entity<Lead>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(20);
                entity.Property(e => e.EstimatedBudget).HasPrecision(18, 2);

                // Global Query Filter for Soft Delete
                entity.HasQueryFilter(e => !e.IsDeleted);

                // One-to-Many Relationship: One Lead can have Many InteractionLogs
                entity.HasMany(e => e.Interactions)
                      .WithOne(i => i.Lead)
                      .HasForeignKey(i => i.LeadId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // 2. Configure InteractionLog Entity Constraints
            builder.Entity<InteractionLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Notes).IsRequired().HasMaxLength(1000);

                // Global Query Filter for Soft Delete
                entity.HasQueryFilter(e => !e.IsDeleted);
            });

            // 3. Configure Property Entity Constraints
            builder.Entity<Property>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(2000);
                entity.Property(e => e.Address).IsRequired().HasMaxLength(300);
                entity.Property(e => e.City).IsRequired().HasMaxLength(100);

                entity.Property(e => e.Price).HasPrecision(18, 2);

                // Global Query Filter for Soft Delete
                entity.HasQueryFilter(e => !e.IsDeleted);

                // Configure One-to-Many Relationship between Agent (ApplicationUser) and Property
                entity.HasOne(e => e.Agent)
                      .WithMany()
                      .HasForeignKey(e => e.AgentId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // 4. Configure Deal Entity Constraints
            builder.Entity<Deal>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.SalePrice).HasPrecision(18, 2);
                entity.Property(e => e.Commission).HasPrecision(18, 2);

                // Global Query Filter for Soft Delete
                entity.HasQueryFilter(e => !e.IsDeleted);
            });

            // 5. Configure Appointment Entity Constraints
            builder.Entity<Appointment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Notes).HasMaxLength(1000);

                // Global Query Filter for Soft Delete
                entity.HasQueryFilter(e => !e.IsDeleted);
            });
        }

        /// <summary>
        /// Intercepts SaveChangesAsync to automatically handle Soft Delete logic and Audit Timestamp updates.
        /// </summary>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var auditEntries = new System.Collections.Generic.List<AuditLog>();

            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                // Skip logging AuditLog entries to prevent infinite recursion
                if (entry.Entity is AuditLog) continue;

                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        entry.Entity.IsDeleted = false;
                        auditEntries.Add(new AuditLog
                        {
                            EntityName = entry.Entity.GetType().Name,
                            Action = "Create",
                            NewValues = entry.DebugView.LongView,
                            CreatedAt = DateTime.UtcNow
                        });
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedAt = DateTime.UtcNow;
                        auditEntries.Add(new AuditLog
                        {
                            EntityName = entry.Entity.GetType().Name,
                            Action = "Update",
                            NewValues = entry.DebugView.LongView,
                            CreatedAt = DateTime.UtcNow
                        });
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.Entity.IsDeleted = true;
                        entry.Entity.LastModifiedAt = DateTime.UtcNow;
                        auditEntries.Add(new AuditLog
                        {
                            EntityName = entry.Entity.GetType().Name,
                            Action = "SoftDelete",
                            CreatedAt = DateTime.UtcNow
                        });
                        break;
                }
            }

            if (auditEntries.Any())
            {
                await AuditLogs.AddRangeAsync(auditEntries, cancellationToken);
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}