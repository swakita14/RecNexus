using System.Data.Entity.ModelConfiguration;
using DiscussionHub.Models;

namespace DiscussionHub.DAL.Configurations
{
    public class DiscussionConfiguration : EntityTypeConfiguration<Discussion>
    {
        public DiscussionConfiguration()
        {
            ToTable("Discussion");

            HasKey(x => x.DiscussionId);

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

            Property(x => x.ArticleLink)
                .HasColumnName("ArticleLink")
                .HasColumnType("nvarchar")
                .HasMaxLength(256)
                .IsRequired();

            Property(x => x.Title)
                .HasColumnName("Title")
                .HasColumnType("nvarchar")
                .HasMaxLength(256)
                .IsRequired();

            Property(x => x.Contents)
                .HasColumnName("Contents")
                .HasColumnType("nvarchar")
                .IsOptional();
        }
    }
}