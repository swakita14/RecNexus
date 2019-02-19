namespace PickUpSports.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using PickUpSports.DAL.Configurations;

    public partial class PickUpContext : DbContext
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
