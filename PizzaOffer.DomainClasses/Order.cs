using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PizzaOffer.DomainClasses
{
    public class Order : BaseEntity
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public string Address { get; set; }
        public OrderStatus Status { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? PaymentDate { get; set; }
        public DateTimeOffset? DeliveredDate { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }

    public enum OrderStatus
    {
        [Display(Name = "Created")]
        Created = 0,
        [Display(Name = "Paid")]
        Paid = 1,
        [Display(Name = "Delivered")]
        Delivered = 2
    }

    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(q => q.Address).HasMaxLength(450);
            builder.Property(q => q.CreatedDate).ValueGeneratedOnAdd().HasDefaultValueSql("SYSDATETIMEOFFSET()");
            builder.HasOne(q => q.User).WithMany(q => q.Orders).HasForeignKey(q => q.UserId);
        }
    }
}
