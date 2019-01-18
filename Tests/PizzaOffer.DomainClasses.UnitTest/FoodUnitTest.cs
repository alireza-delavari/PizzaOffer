using AutoFixture.Xunit2;
using System;
using Xunit;

namespace PizzaOffer.DomainClasses.UnitTest
{
    public class FoodUnitTest
    {
        [Theory , AutoData]
        public void Id_GetterAndSetter_ShouldBeAccessible(int id)
        {
            var food = new Food() { Id = id };
            Assert.Equal(food.Id, id);
        }
    }
}
