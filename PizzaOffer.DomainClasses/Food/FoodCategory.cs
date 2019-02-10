using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaOffer.DomainClasses
{
    public class FoodCategory : BaseEntity
    {
        public FoodCategory()
        {
            Foods = new HashSet<Food>();
        }

        public string Name { get; set; }

        public virtual ICollection<Food> Foods { get; set; }
    }

    public class FoodCategoryConfiguration : IEntityTypeConfiguration<FoodCategory>
    {
        public void Configure(EntityTypeBuilder<FoodCategory> builder)
        {
            builder.Property(q => q.Name).HasMaxLength(100);
        }
    }
}
