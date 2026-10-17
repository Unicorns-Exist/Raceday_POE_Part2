using Microsoft.EntityFrameworkCore;

namespace Raceday_POE2.Data
{
    /// <summary>
    /// RaceDayContext is the bridge between our C# application
    /// and the SQL Server database.
    ///
    /// It inherits from DbContext, which is a class provided by
    /// Entity Framework Core.
    ///
    /// We will later add properties for all six RaceDay entities:
    /// User, Event, Category, EventCategory, Enrolment and Result.
    /// </summary>
    public class RaceDayContext : DbContext
    {
        /// <summary>
        /// This constructor receives the database options that
        /// we configure in Program.cs.
        ///
        /// The options include which database provider to use
        /// and how to connect to the database.
        /// </summary>
        public RaceDayContext(
            DbContextOptions<RaceDayContext> options)
            : base(options)
        {
        }

        // The DbSet properties for our six entities will be added
        // here in the next stage.
    }
}