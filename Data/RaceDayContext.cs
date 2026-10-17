using Microsoft.EntityFrameworkCore;
using Raceday_POE2.Models;

namespace Raceday_POE2.Data
{
    /// <summary>
    /// The database context connects our six C# models
    /// to their corresponding SQL Server tables.
    /// </summary>
    public class RaceDayContext : DbContext
    {
        public RaceDayContext(
            DbContextOptions<RaceDayContext> options)
            : base(options)
        {
        }

        // Each DbSet represents a table that EF Core manages.
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<EventCategory> EventCategories { get; set; }
        public DbSet<Enrolment> Enrolments { get; set; }
        public DbSet<Result> Results { get; set; }

        /// <summary>
        /// Configure relationships, uniqueness and database rules.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---------------------------------------------
            // USER RULES
            // ---------------------------------------------

            // Prevent two accounts from using the same email.
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Only Organiser or Participant is allowed as a role.
            modelBuilder.Entity<User>()
                .ToTable(t => t.HasCheckConstraint(
                    "CK_Users_Role",
                    "[Role] IN ('Organiser', 'Participant')"));

            // ---------------------------------------------
            // EVENT RULES
            // ---------------------------------------------

            // An event belongs to one Organiser.
            // One Organiser can create multiple events.
            modelBuilder.Entity<Event>()
                .HasOne(e => e.Organiser)
                .WithMany(u => u.OrganisedEvents)
                .HasForeignKey(e => e.OrganiserID)
                .OnDelete(DeleteBehavior.Restrict);

            // Event distance must be positive.
            modelBuilder.Entity<Event>()
                .ToTable(t => t.HasCheckConstraint(
                    "CK_Events_DistanceKm",
                    "[DistanceKm] > 0"));

            // Store distance using two decimal places.
            modelBuilder.Entity<Event>()
                .Property(e => e.DistanceKm)
                .HasPrecision(6, 2);

            // Only Run, Walk or Cycle is allowed.
            modelBuilder.Entity<Event>()
                .ToTable(t => t.HasCheckConstraint(
                    "CK_Events_EventType",
                    "[EventType] IN ('Run', 'Walk', 'Cycle')"));

            // ---------------------------------------------
            // CATEGORY RULES
            // ---------------------------------------------

            // Category type must be Age or Distance.
            modelBuilder.Entity<Category>()
                .ToTable(t => t.HasCheckConstraint(
                    "CK_Categories_CategoryType",
                    "[CategoryType] IN ('Age', 'Distance')"));

            modelBuilder.Entity<Category>()
                .Property(c => c.DistanceKm)
                .HasPrecision(6, 2);

            // ---------------------------------------------
            // EVENT-CATEGORY RELATIONSHIP
            // ---------------------------------------------

            // One Event can offer many EventCategory records.
            modelBuilder.Entity<EventCategory>()
                .HasOne(ec => ec.Event)
                .WithMany(e => e.EventCategories)
                .HasForeignKey(ec => ec.EventID)
                .OnDelete(DeleteBehavior.Restrict);

            // One Category can be offered at many events.
            modelBuilder.Entity<EventCategory>()
                .HasOne(ec => ec.Category)
                .WithMany(c => c.EventCategories)
                .HasForeignKey(ec => ec.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);

            // Do not allow the same category to be linked
            // to the same event more than once.
            modelBuilder.Entity<EventCategory>()
                .HasIndex(ec => new { ec.EventID, ec.CategoryID })
                .IsUnique();

            modelBuilder.Entity<EventCategory>()
                .Property(ec => ec.EntryFee)
                .HasPrecision(10, 2);

            modelBuilder.Entity<EventCategory>()
                .ToTable(t => t.HasCheckConstraint(
                    "CK_EventCategories_EntryFee",
                    "[EntryFee] >= 0"));

            modelBuilder.Entity<EventCategory>()
                .ToTable(t => t.HasCheckConstraint(
                    "CK_EventCategories_MaxParticipants",
                    "[MaxParticipants] IS NULL OR [MaxParticipants] > 0"));

            // ---------------------------------------------
            // ENROLMENT RELATIONSHIPS
            // ---------------------------------------------

            // One Participant can have many enrolments.
            modelBuilder.Entity<Enrolment>()
                .HasOne(en => en.Participant)
                .WithMany(u => u.Enrolments)
                .HasForeignKey(en => en.ParticipantID)
                .OnDelete(DeleteBehavior.Restrict);

            // Each enrolment selects one EventCategory.
            modelBuilder.Entity<Enrolment>()
                .HasOne(en => en.EventCategory)
                .WithMany(ec => ec.Enrolments)
                .HasForeignKey(en => en.EventCategoryID)
                .OnDelete(DeleteBehavior.Restrict);

            // The enrolment date is generated by SQL Server
            // when no date is supplied by the application.
            modelBuilder.Entity<Enrolment>()
                .Property(en => en.EnrolmentDate)
                .HasDefaultValueSql("GETDATE()");

            // The default enrolment status is Active.
            modelBuilder.Entity<Enrolment>()
                .Property(en => en.Status)
                .HasDefaultValue("Active");

            modelBuilder.Entity<Enrolment>()
                .ToTable(t => t.HasCheckConstraint(
                    "CK_Enrolments_Status",
                    "[Status] IN ('Active', 'Cancelled')"));

            // ---------------------------------------------
            // RESULT RELATIONSHIP
            // ---------------------------------------------

            // One enrolment can have zero or one result.
            modelBuilder.Entity<Result>()
                .HasOne(r => r.Enrolment)
                .WithOne(en => en.Result)
                .HasForeignKey<Result>(r => r.EnrolmentID)
                .OnDelete(DeleteBehavior.Restrict);

            // A result must have a positive finishing position.
            modelBuilder.Entity<Result>()
                .ToTable(t => t.HasCheckConstraint(
                    "CK_Results_FinishingPosition",
                    "[FinishingPosition] > 0"));

            // Ensure only one result is recorded per enrolment.
            modelBuilder.Entity<Result>()
                .HasIndex(r => r.EnrolmentID)
                .IsUnique();
        }
    }
}