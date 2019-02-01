using System.Data.Entity.ModelConfiguration;
using DiscussionHub.Models;

namespace DiscussionHub.DAL.Configurations
{
    public class DiscussionHubUserConfiguration : EntityTypeConfiguration<DiscussionHubUser>
    {
        public DiscussionHubUserConfiguration()
        {
            ToTable("DiscussionHubUser");

            HasKey(x => x.UserId);

            Property(x => x.UserId)
                .HasColumnName("UserID")
                .HasColumnType("int")
                .IsRequired();

            Property(x => x.Browser)
                .HasColumnName("Browser")
                .HasColumnType("nvarchar")
                .HasMaxLength(256)
                .IsRequired();

            Property(x => x.FName)
                .HasColumnName("FName")
                .HasColumnType("nvarchar")
                .HasMaxLength(128)
                .IsOptional();

            Property(x => x.LName)
                .HasColumnName("LName")
                .HasColumnType("nvarchar")
                .HasMaxLength(256)
                .IsOptional();

            Property(x => x.Email)
                .HasColumnName("Email")
                .HasColumnType("nvarchar")
                .HasMaxLength(128)
                .IsOptional();

            Property(x => x.LoginPref)
                .HasColumnName("LoginPref")
                .HasColumnType("nvarchar")
                .HasMaxLength(256)
                .IsOptional();

            Property(x => x.VoteTotal)
                .HasColumnName("VoteTotal")
                .HasColumnType("bigint")
                .IsOptional();

            Property(x => x.About)
                .HasColumnName("About")
                .HasColumnType("text")
                .IsOptional();

            Property(x => x.Pseudonym)
                .HasColumnName("Pseudonym")
                .HasColumnType("nvarchar")
                .HasMaxLength(256)
                .IsOptional();

        }
    }
}