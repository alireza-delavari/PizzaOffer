using System;

namespace PizzaOffer.DomainClasses
{
    public class Food
    {
        public int Id { get; set; }
        public string FoodName { get; set; }
        public string Description { get; set; }
        public Int64 Price { get; set; }
        public bool IsActive { get; set; }
        public int FoodCategoryId { get; set; }

        public virtual FoodCategory FoodCategory { get; set; }

    }
}
