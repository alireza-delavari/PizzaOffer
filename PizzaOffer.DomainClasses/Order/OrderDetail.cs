using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaOffer.DomainClasses
{
    public class OrderDetail : BaseEntity
    {
        public int Count { get; set; }
        public int OrderId { get; set; }
        public int FoodId { get; set; }

        public virtual Order Order { get; set; }
        public virtual Food Food { get; set; }
    }

    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.HasOne(q => q.Order).WithMany(q => q.OrderDetails).HasForeignKey(q => q.OrderId);
            builder.HasOne(q => q.Food).WithMany(q => q.OrderDetails).HasForeignKey(q => q.FoodId);
        }
    }
}
