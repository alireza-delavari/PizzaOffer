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
        }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool IsPhoneConfirmed { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool IsUserActive { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public DateTimeOffset? LastLoginDate { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(q => q.UserName).HasMaxLength(50).IsRequired();
            builder.HasIndex(q => q.UserName).IsUnique();

            builder.Property(q => q.Password).HasMaxLength(100).IsRequired();
            builder.Property(q => q.DisplayName).HasMaxLength(50);

            builder.Property(q => q.PhoneNumber).HasMaxLength(20);
            builder.HasIndex(q => q.PhoneNumber).IsUnique();

            builder.Property(q => q.Email).HasMaxLength(254);
            builder.HasIndex(q => q.Email).IsUnique();

            builder.Property(q => q.CreatedDate).ValueGeneratedOnAdd().HasDefaultValueSql("SYSDATETIMEOFFSET()");
            builder.Property(q => q.UpdatedDate).ValueGeneratedOnAddOrUpdate().HasDefaultValueSql("SYSDATETIMEOFFSET()");
        }
    }
}
