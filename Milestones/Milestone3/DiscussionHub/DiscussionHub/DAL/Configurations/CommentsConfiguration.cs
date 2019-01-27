using System.Data.Entity.ModelConfiguration;
using DiscussionHub.Models;

namespace DiscussionHub.DAL.Configurations
{
    public class CommentsConfiguration : EntityTypeConfiguration<Comments>
    {
        public CommentsConfiguration()
        {
            ToTable("Comments");

            HasKey(x => x.CommentId);

            Property(x => x.CommentId)
                .HasColumnName("CommentID")
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

            Property(x => x.GifRequest)
                .HasColumnName("GIFRequest")
                .HasColumnType("nvarchar")
                .HasMaxLength(200)
                .IsOptional();

            Property(x => x.ImageRequest)
                .HasColumnName("ImageRequest")
                .HasColumnType("nvarchar")
                .HasMaxLength(200)
                .IsOptional();

            Property(x => x.UserId)
                .HasColumnName("UserID")
                .HasColumnType("int")
                .IsRequired();

            Property(x => x.DiscussionId)
                .HasColumnName("DiscussionID")
                .HasColumnType("int")
                .IsRequired();

        }
    }

}