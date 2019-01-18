using System;
using System.Collections.Generic;

namespace PizzaOffer.DomainClasses
{
    public class Food
    {
        public Food()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public string FoodName { get; set; }
        public string Description { get; set; }
        public Int64 Price { get; set; }
        public bool IsActive { get; set; }
        public int FoodCategoryId { get; set; }

        public virtual FoodCategory FoodCategory { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
