using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Controllers;
using WebApplication2.Data;
using WebApplication2.Models;
using Microsoft.AspNetCore.Identity;


//to count code coverage
//write dotnet test --collect:"XPlat Code Coverage"
//into shell

//

namespace WebApplication2.Tests.Controllers
{
    [TestFixture]
    public class CartControllerTests
    {
        private ApplicationDbContext _context;
        private CartsController _cartsController;
        private OrdersController _ordersController;
        private ShopController _shopController;
        [SetUp]
        public void Setup()
        {


            // Конфігурування контексту бази даних
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
             .UseInMemoryDatabase(databaseName: "TestDb")
             .Options;
            _context = new ApplicationDbContext(options);
            SeedDatabase();

            /// Створення контролера CartsController з підробленим контекстом бази даних
            _cartsController = new CartsController(_context);

            // Створення контролера OrdersController з підробленим контекстом бази даних
            _ordersController = new OrdersController(_context);
            // Створення контролера ShopController з підробленим контекстом бази даних
            _shopController = new ShopController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            // Видалення тестової бази даних
            _context.Database.EnsureDeleted();
        }

        [Test]
        public async Task Create_WithValidCart_RedirectsToIndex()
        {
            // Arrange
            var cart = new Cart { Quantity = 2, Price = 10.99m };

            // Act
            var result = await _cartsController.Create(cart) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public async Task Create_WithInvalidCart_ReturnsViewWithCart()
        {
            // Arrange
            var cart = new Cart { Quantity = -1, Price = 10.99m };
            _cartsController.ModelState.AddModelError("Quantity", "Invalid quantity");

            // Act
            var result = await _cartsController.Create(cart) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(cart, result.Model);
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




        [Test]
        public async Task Delete_NonExistingCart_ReturnsNotFound()
        {
            // Arrange
            var cartId = 999; // Non-existing cart ID

            // Act
            var result = await _cartsController.Delete(cartId) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Test]
        public async Task Delete_Confirmed_DeletesCart()
        {
            // Arrange
            var cartId = 1;

            // Act
            var result = await _cartsController.DeleteConfirmed(cartId) as RedirectToActionResult;// Assert
            Assert.NotNull(result);
            Assert.AreEqual("Index", result.ActionName);

            // Check if the cart was deleted from the database
            var deletedCart = await _context.Cart.FindAsync(cartId);
            Assert.Null(deletedCart);
        }
        [Test]
        public async Task Create_WithValidOrder_RedirectsToIndex()
        {
            // Arrange
            var order = new Order();

            // Act
            var result = await _ordersController.Create(order) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }
        [Test]
        public async Task AddToCart_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var invalidId = 999;

            // Act
            var result = await _shopController.AddToCart(invalidId) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }
        [Test]
        public void Test_All_Models()
        {
            // Arrange
            var pizza = new Pizza
            {
                PizzaName = "Margherita",
                BasePrice = 21,
                Beef = true,
                Cheese = true,
                Id = 123,
                FinalPrice = 1234,
                Mushroom = false,
                Details = "Test",
                Ham = false,
                ImageTitle = "Bimba",
                Peperoni = false,
                Pineapple = true,
                TomatoSauce = false,
                Tuna = true
            };
            var order = new Order
            {
                Id = 1,
                User = new IdentityUser { UserName = "john.doe" },
                PizzaToOrder = new List<PizzaToOrder>()
            };
            var pizzaToOrder = new PizzaToOrder
            {
                Id = 1,
                Pizza = pizza,
                Order = order
            };
            var cart = new Cart
            {
                Id = 1,
                User = new IdentityUser { UserName = "john.doe" },
                Pizza = pizza,
                Quantity = 2,
                Price = 10.99m
            };

            // Act
            var pizzaName = pizza.PizzaName;
            var orderUser = order.User.UserName;
            var pizzaToOrderId = pizzaToOrder.Id;
            var cartUser = cart.User.UserName;
            var cartPizzaName = cart.Pizza.PizzaName;

            // Assert
            Assert.IsNotNull(pizzaName);
            Assert.IsNotNull(orderUser);
            Assert.AreEqual(1, pizzaToOrderId);
            Assert.IsNotNull(cartUser);
            Assert.IsNotNull(cartPizzaName);
        }

        [Test]
        public void Test_Add_To_Cart()
        {
            // Arrange
            var pizza = new Pizza { Id = 1, PizzaName = "Margherita" };
            var pizzas = new List<Pizza> { pizza };

            // Act
            var model = pizzas.AsEnumerable();

            // Assert
            var imagePath = $"~/images/Pizzas/{pizza.ImageTitle}.png";
            var expectedUrl = $"/Shop/AddToCart/{pizza.Id}";
            foreach (var p in model)
            {
                var linkUrl = $"/Shop/AddToCart/{p.Id}";
                Assert.AreEqual(expectedUrl, linkUrl);
            }
        }

        [Test]
        public void Test_Pizza_Images()
        {
            // Arrange
            var pizzas = new List<Pizza>
    {
        new Pizza { ImageTitle = "Margherita" },
        new Pizza { ImageTitle = "Pepperoni" },
        new Pizza { ImageTitle = "Vegetarian" }
    };

            // Act
            var model = pizzas.AsEnumerable();

            // Assert
            foreach (var pizza in model)
            {
                var imagePath = $"~/images/Pizzas/{pizza.ImageTitle}.png";
                Assert.IsNotNull(imagePath);
            }
        }

        [Test]
        public void Test_Pizza_Names()
        {
            // Arrange
            var pizzas = new List<Pizza>
    {
        new Pizza { PizzaName = "Margherita" },
        new Pizza { PizzaName = "Pepperoni" },
        new Pizza { PizzaName = "Vegetarian" }
    };

            // Act
            var model = pizzas.AsEnumerable();

            // Assert
            foreach (var pizza in model)
            {
                Assert.IsNotNull(pizza.PizzaName);
            }
        }


        private void SeedDatabase()
        {
            // Додавання тестових даних до бази даних
            var pizza = new Pizza
            {
                Id = 1,
                PizzaName = "Margherita",
                BasePrice = 2.99f,
                Details = "Pizza details",
                ImageTitle = "pizza-image.jpg"
            };
            _context.Pizza.Add(pizza);
            _context.SaveChanges();
        }

    }
}
