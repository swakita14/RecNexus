using System.Data.Entity;
using DiscussionHub.DAL.Configurations;
using DiscussionHub.Models;

namespace DiscussionHub.DAL
{
    public class DiscussionHubContext : DbContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<Discussions> Discussions { get; set; }

        public DiscussionHubContext() : base("name=DiscussionHubContext") { }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            // Model configurations to map entity to model
            builder.Configurations.Add(new UsersConfiguration());
            builder.Configurations.Add(new CommentsConfiguration());
            builder.Configurations.Add(new DiscussionsConfiguration());
        }

    }
}