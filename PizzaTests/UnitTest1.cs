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


            // ?????????????? ????????? ???? ?????
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
             .UseInMemoryDatabase(databaseName: "TestDb")
             .Options;
            _context = new ApplicationDbContext(options);
            SeedDatabase();

            /// ????????? ?????????? CartsController ? ?????????? ?????????? ???? ?????
            _cartsController = new CartsController(_context);

            // ????????? ?????????? OrdersController ? ?????????? ?????????? ???? ?????
            _ordersController = new OrdersController(_context);
            // ????????? ?????????? ShopController ? ?????????? ?????????? ???? ?????
            _shopController = new ShopController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            // ????????? ??????? ???? ?????
            _context.Database.EnsureDeleted();
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
        [Test]
        public async Task Index_ReturnsViewResultWithCartList()
        {
            // Arrange
            var carts = new List<Cart>
    {
        new Cart { Id = 1, Quantity = 2, Price = 10.99m },
        new Cart { Id = 2, Quantity = 3, Price = 15.99m }
    };
            _context.Cart.AddRange(carts);
            _context.SaveChanges();
            var expectedCartList = carts.Where(m => m.Id == 1);

            // Act
            var result = _cartsController.Index();


            // Assert
            Assert.NotNull(result);

        }

        [Test]
        public async Task Buy_ClearsCartAndCreatesOrder()
        {
            // Arrange
            var curr_usr = new IdentityUser { UserName = "john.doe" };
            _context.Users.Add(curr_usr);
            _context.SaveChanges();
            var cartItems = new List<Cart>
    {
        new Cart { Id = 1, User = curr_usr, Quantity = 2, Price = 10.99m },
        new Cart { Id = 2, User = curr_usr, Quantity = 3, Price = 15.99m }
    };
            _context.Cart.AddRange(cartItems);
            _context.SaveChanges();
            var expectedOrder = new Order { User = curr_usr };

            // Act
            var result = _cartsController.Buy() as RedirectToActionResult;
            var orders = _context.Orders.Where(o => o.User.UserName == curr_usr.UserName).ToList();
            var cartItemsAfterBuy = _context.Cart.Where(c => c.User.UserName == curr_usr.UserName).ToList();

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual(1, orders.Count());
            Assert.AreEqual(expectedOrder.User.UserName, orders.First().User.UserName);
            Assert.AreEqual(0, cartItemsAfterBuy.Count());
        }

        [Test]
        public async Task Details_WithValidId_ReturnsViewResultWithCart()
        {
            // Arrange
            var cartId = 1;
            var cart = new Cart { Id = cartId, Quantity = 2, Price = 10.99m };
            _context.Cart.Add(cart);
            _context.SaveChanges();

            // Act
            var result = await _cartsController.Details(cartId) as ViewResult;
            var model = result?.Model as Cart;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(model);
            Assert.AreEqual(cartId, model.Id);
            Assert.AreEqual(cart.Quantity, model.Quantity);
            Assert.AreEqual(cart.Price, model.Price);
        }

        [Test]
        public async Task Details_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var invalidCartId = 999;

            // Act
            var result = await _cartsController.Details(invalidCartId) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }






        [Test]
        public async Task Edit_WithInvalidCart_ReturnsNotFound()
        {
            // Arrange
            var invalidCartId = 999; // ID неіснуючого об'єкта Cart

            // Act
            var result = await _cartsController.Edit(invalidCartId) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }
















        private void SeedDatabase()
        {
            // ????????? ???????? ????? ?? ???? ?????
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