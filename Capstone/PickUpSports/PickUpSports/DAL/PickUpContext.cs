using System.Data.Entity;
using PickUpSports.DAL.Configurations;
using PickUpSports.Models;

namespace PickUpSports.DAL
{
    public class PickUpContext : DbContext
    {
        public PickUpContext()
            : base("name=PickUpContext")
        {

        }

        public DbSet<Contact> Contacts { get; set; }
        protected override void OnModelCreating(DbModelBuilder builder)
        {
            // Model configurations to map entity to model
            builder.Configurations.Add(new ContactConfiguration());
        }
    }
}
