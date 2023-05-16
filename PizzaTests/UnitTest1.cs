using NUnit.Framework;
using WebApplication2.Models;

namespace PizzaTests
{
    public class PizzaTests
    {
        [SetUp]
        public void Setup()
        {
            // Add any setup code or initialization here
        }

        [Test]
        public void Pizza_Name_Should_Not_Be_Null()
        {
            // Arrange
            var pizza = new Pizza { PizzaName = "Margherita" };

            // Act
            var name = pizza.PizzaName;

            // Assert
            Assert.IsNotNull(name);
        }

        [Test]
        public void Pizza_Price_Should_Be_Greater_Than_Zero()
        {
            // Arrange
            const float V = 10;
            var pizza = new Pizza { FinalPrice = V };

            // Act
            var price = pizza.FinalPrice;

            // Assert
            Assert.Greater(price, 0);
        }
    }
}
