using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using PickUpSports.Models;
using System.Web;

namespace PickUpSports.DAL.Configurations
{
    public class ContactConfiguration : EntityTypeConfiguration<Contact>
    {
        public ContactConfiguration()
        {
            ToTable("Contact");

            HasKey(x => x.ContactId);

            Property(x => x.ContactId)
                .HasColumnName("ContactId")
                .HasColumnType("int")
                .IsRequired();

            Property(x => x.Username)
                .HasColumnName("Username")
                .HasColumnType("nvarchar")
                .HasMaxLength(256)
                .IsRequired();

            Property(x => x.FirstName)
                .HasColumnName("FirstName")
                .HasColumnType("nvarchar")
                .HasMaxLength(128)
                .IsRequired();

            Property(x => x.LastName)
                .HasColumnName("LastName")
                .HasColumnType("nvarchar")
                .HasMaxLength(256)
                .IsRequired();

            Property(x => x.Email)
                .HasColumnName("Email")
                .HasColumnType("nvarchar")
                .HasMaxLength(128)
                .IsRequired();

            Property(x => x.PhoneNumber)
                .HasColumnName("PhoneNumber")
                .HasColumnType("nvarchar")
                .IsRequired();

            Property(x => x.Address1)
                .HasColumnName("Address1")
                .HasColumnType("nvarchar")
                .HasMaxLength(256)
                .IsRequired();

            Property(x => x.Address2)
               .HasColumnName("Address2")
               .HasColumnType("nvarchar")
               .HasMaxLength(256)
               .IsOptional();

            Property(x => x.City)
                .HasColumnName("City")
                .HasColumnType("nvarchar")
                .HasMaxLength(256)
                .IsRequired();

            Property(x => x.State)
               .HasColumnName("State")
               .HasColumnType("nvarchar")
               .HasMaxLength(256)
               .IsRequired();

            Property(x => x.ZipCode)
              .HasColumnName("ZipCode")
              .HasColumnType("nvarchar")
              .HasMaxLength(256)
              .IsRequired();
        }

    }
}