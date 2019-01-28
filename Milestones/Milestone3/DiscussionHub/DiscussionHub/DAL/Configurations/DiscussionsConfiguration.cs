using System.Data.Entity.ModelConfiguration;
using DiscussionHub.Models;

namespace DiscussionHub.DAL.Configurations
{
    public class DiscussionsConfiguration : EntityTypeConfiguration<Discussions>
    {
        public DiscussionsConfiguration()
        {
            ToTable("Discussions");

            // Change as needed for primary key
            HasKey(x => x.DiscussionId);

            // Configure any foreign keys

            Property(x => x.DiscussionId)
                .HasColumnName("DiscussionID")
                .HasColumnType("int")
                .IsRequired();

            Property(x => x.VoteCount)
                .HasColumnName("VoteCount")
                .HasColumnType("bigint")
                .IsRequired();

            Property(x => x.UpvoteCount)
                .HasColumnName("UpvoteCount")
                .HasColumnType("bigint")
                .IsRequired();

            Property(x => x.DownvoteCount)
                .HasColumnName("DownvoteCount")
                .HasColumnType("bigint")
                .IsRequired();

            Property(x => x.CommentCount)
                .HasColumnName("CommentCount")
                .HasColumnType("bigint")
                .IsRequired();

            Property(x => x.TotalViews)
                .HasColumnName("TotalViews")
                .HasColumnType("bigint")
                .IsRequired();

            Property(x => x.Rank)
                .HasColumnName("Rank")
                .HasColumnType("int")
                .IsRequired();

            Property(x => x.UserId)
                .HasColumnName("UserID")
                .HasColumnType("int")
                .IsRequired();

        }
    }

}