using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaOffer.DomainClasses
{
    public class UserRole : IBaseEntity
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }

        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }

    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasOne(q => q.User).WithMany(q => q.UserRoles).HasForeignKey(q => q.UserId);
            builder.HasOne(q => q.Role).WithMany(q => q.UserRoles).HasForeignKey(q => q.RoleId);
        }
    }
}
