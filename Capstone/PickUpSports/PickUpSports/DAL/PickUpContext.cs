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
<<<<<<< HEAD
        public DbSet<Sport> Sports { get; set; }
        public DbSet<SportPreference> SportPreferences { get; set; }
=======
        public DbSet<Location> Locations { get; set; }

>>>>>>> c381d77a0a98f95f60a8a92897e31d672f7a0b5c

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            // Model configurations to map entity to model
            builder.Configurations.Add(new ContactConfiguration());
            builder.Configurations.Add(new TimePreferenceConfiguration());
            builder.Configurations.Add(new BusinessHoursConfiguration());
            builder.Configurations.Add(new VenueConfiguration());
            builder.Configurations.Add(new ReviewConfiguration());
<<<<<<< HEAD
            builder.Configurations.Add(new SportConfiguration());
            builder.Configurations.Add(new SportPreferenceConfiguration());
=======
            builder.Configurations.Add(new LocationConfiguration());
>>>>>>> c381d77a0a98f95f60a8a92897e31d672f7a0b5c
        }
    }
}
