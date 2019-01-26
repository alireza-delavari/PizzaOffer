using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaOffer.DomainClasses
{
    public class User : BaseEntity
    {
        public User()
        {
            //Todo: check if HashSet is good(null object patterns)(dissabling lazy loading)
            Orders = new HashSet<Order>();
            UserRoles = new HashSet<UserRole>();
            UserTokens = new HashSet<UserToken>();
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string AvatarImage { get; set; }
        public int AccessFailedCount { get; set; }
        public bool IsPhoneConfirmed { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool IsUserActive { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public DateTimeOffset? LastVisitDate { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        
        /// <summary>
        /// every time the user changes his Password,
        /// or an admin changes his Roles or stat/IsActive,
        /// create a new `SerialNumber` GUID and store it in the DB.
        /// </summary>
        public string SerialNumber { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<UserToken> UserTokens { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(q => q.Username).HasMaxLength(100).IsRequired();
            builder.HasIndex(q => q.Username).IsUnique();

            builder.Property(q => q.Password).HasMaxLength(100).IsRequired();
            builder.Property(q => q.DisplayName).HasMaxLength(100);
            builder.Property(e => e.SerialNumber).HasMaxLength(450);
            builder.Property(e => e.AvatarImage).HasMaxLength(2048);

            builder.Property(q => q.PhoneNumber).HasMaxLength(20);
            builder.HasIndex(q => q.PhoneNumber).IsUnique();

            builder.Property(q => q.Email).HasMaxLength(254);
            builder.HasIndex(q => q.Email).IsUnique();

            builder.Property(q => q.CreatedDate).ValueGeneratedOnAdd().HasDefaultValueSql("SYSDATETIMEOFFSET()");
            builder.Property(q => q.UpdatedDate).ValueGeneratedOnAddOrUpdate().HasDefaultValueSql("SYSDATETIMEOFFSET()");
        }
    }
}
