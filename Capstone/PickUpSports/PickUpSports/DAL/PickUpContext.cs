using System.Data.Entity;
using PickUpSports.DAL.Configurations;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.DAL
{
    public class PickUpContext : DbContext
    {
        public PickUpContext() : base("name=PickUpContext"){}

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<TimePreference> TimePreferences { get; set; }
        public DbSet<BusinessHours> BusinessHours { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Sport> Sports { get; set; }
        public DbSet<SportPreference> SportPreferences { get; set; }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            // Model configurations to map entity to model
            builder.Configurations.Add(new ContactConfiguration());
            builder.Configurations.Add(new TimePreferenceConfiguration());
            builder.Configurations.Add(new BusinessHoursConfiguration());
            builder.Configurations.Add(new VenueConfiguration());
            builder.Configurations.Add(new ReviewConfiguration());
            builder.Configurations.Add(new SportConfiguration());
            builder.Configurations.Add(new SportPreferenceConfiguration());
        }
    }
}
