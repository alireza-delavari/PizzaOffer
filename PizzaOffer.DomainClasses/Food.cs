using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace PizzaOffer.DomainClasses
{
    public class Food : BaseEntity
    {
        public Food()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public string FoodName { get; set; }
        public string Description { get; set; }
        public Int64 Price { get; set; }
        public bool IsActive { get; set; }
        public int FoodCategoryId { get; set; }

        public virtual FoodCategory FoodCategory { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }

    public class FoodConfiguration : IEntityTypeConfiguration<Food>
    {
        public void Configure(EntityTypeBuilder<Food> builder)
        {
            builder.Property(q => q.FoodName).HasMaxLength(50).IsRequired();
            builder.Property(q => q.Description).HasColumnType("nvarchar(max)");
            builder.HasOne(q => q.FoodCategory).WithMany(q => q.Foods).HasForeignKey(q => q.FoodCategoryId);
        }
    }
}
