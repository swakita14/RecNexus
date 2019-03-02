using System.Data.Entity;
using PickUpSports.DAL.Configurations;
using PickUpSports.Models;

namespace PickUpSports.DAL
{
    public class PickUpContext : DbContext
    {
        public PickUpContext(): base("name=PickUpContext"){
        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<TimePreference> TimePreferences { get; set; }
        public DbSet<BusinessHours> BusinessHours { get; set; }
        public DbSet<Venue> Venues { get; set; }


        protected override void OnModelCreating(DbModelBuilder builder)
        {
            // Model configurations to map entity to model
            builder.Configurations.Add(new ContactConfiguration());
            builder.Configurations.Add(new TimePreferenceConfiguration());
            builder.Configurations.Add(new BusinessHoursConfiguration());
            builder.Configurations.Add(new VenueConfiguration());

        }

        public System.Data.Entity.DbSet<PickUpSports.Models.TimePreference> TimePreferences { get; set; }
    }
}
