using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaOffer.DomainClasses
{
    public class UserToken : BaseEntity
    {

        public string AccessTokenHash { get; set; }
        public string RefreshTokenIdHash { get; set; }
        public string RefreshTokenIdHashSource { get; set; }
        public DateTimeOffset AccessTokenExpiresDateTime { get; set; }
        public DateTimeOffset RefreshTokenExpiresDateTime { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
    }

    public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            builder.HasOne(q => q.User).WithMany(q => q.UserTokens).HasForeignKey(q=>q.UserId);
            builder.Property(q => q.AccessTokenHash).HasMaxLength(450).IsRequired();
            builder.Property(q => q.RefreshTokenIdHash).HasMaxLength(450).IsRequired();
            builder.Property(q => q.RefreshTokenIdHashSource).HasMaxLength(450);
        }
    }
}
