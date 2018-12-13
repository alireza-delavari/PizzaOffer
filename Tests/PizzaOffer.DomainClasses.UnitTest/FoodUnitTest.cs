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
            foreach (var item in food.GetType().GetProperties())
            {
                //item.MemberType=System.Reflection.MemberTypes.Property

            }
            Assert.Equal(food.Id, id);
        }
    }
}
