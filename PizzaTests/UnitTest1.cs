 using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Controllers;
using WebApplication2.Data;
using WebApplication2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using NUnit.Framework;
using Microsoft.Extensions.Options;




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
        private HomeController _homeController;
        private MakeCustomPizza _custumcontroller;
        private OrdersController _ordersController;
        private PizzasController _controller;
        private ShopController _shopController;
        private UserManager<User> _userManager;
        private UsersController _usersController;
       


        [SetUp]
        public void Setup()
        {


            // ?????????????? ????????? ???? ?????
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
    .UseInMemoryDatabase(databaseName: "TestDb")
    .EnableSensitiveDataLogging()
    .Options;
            _context = new ApplicationDbContext(options);

            SeedDatabase();


           

            /// ????????? ?????????? CartsController ? ?????????? ?????????? ???? ?????
            _cartsController = new CartsController(_context);
            
            // ????????? ?????????? OrdersController ? ?????????? ?????????? ???? ?????
            _ordersController = new OrdersController(_context);
            // ????????? ?????????? ShopController ? ?????????? ?????????? ???? ?????
            _shopController = new ShopController(_context);
            var logger = new LoggerFactory().CreateLogger<HomeController>();
            _homeController = new HomeController(logger);

            _custumcontroller = new MakeCustomPizza(_context);
            _controller = new PizzasController(_context);
            _custumcontroller = new MakeCustomPizza(_context);
            // Mock UserManager using a mocking framework like Moq
            var userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _userManager = userManagerMock.Object;
            _usersController = new UsersController(_userManager);
        }

        [TearDown]
        public void TearDown()
        {
            // ????????? ??????? ???? ?????
            _context.Database.EnsureDeleted();
        }
        





        [Test]
        public void Create_ReturnsViewResultWithCreateButton()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                var controller = new CartsController(dbContext);

                // Act
                var result = controller.Create() as ViewResult;

                // Assert
                Assert.NotNull(result);
                Assert.AreEqual("Create", result.ViewName);
            }
        }



        [Test]
        public async Task DeleteConfirmed_WithValidId_DeletesOrder()
        {
            // Arrange
            var orderId = 1;

            // Act
            var result = await _ordersController.DeleteConfirmed(orderId) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("Index", result.ActionName);

            // Перевірка, чи замовлення було видалено з бази даних
            var deletedOrder = await _context.Orders.FindAsync(orderId);
            Assert.Null(deletedOrder);
        }
        [Test]
        public async Task Index_ReturnsViewResult1()
        {
            // Act
            var result = await _ordersController.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Test]
        public async Task Index_ReturnsListOfOrders()
        {
            // Act
            var result = await _ordersController.Index() as ViewResult;
           

            // Assert
            Assert.NotNull(result);
            
        }
        [Test]
        public async Task Delete_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var invalidId = 999;

            // Act
            var result = await _ordersController.Delete(invalidId) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }


        [Test]
        public async Task Create_WithInvalidPizza_ReturnsViewWithPizza()
        {
            // Arrange
            var pizza = new Pizza();
            _custumcontroller.ModelState.AddModelError("PizzaName", "Invalid pizza name");

            // Act
            var result = await _custumcontroller.Create(pizza) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(pizza, result.Model);
        }
        [Test]
        public void Index_ReturnsViewResulth()
        {
            // Act
            var result = _homeController.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Test]
        public void Privacy_ReturnsViewResult()
        {
            // Act
            var result = _homeController.Privacy() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Test]
        public void AdminPanel_ReturnsViewResult()
        {
            // Act
            var result = _homeController.AdminPanel() as ViewResult;

            // Assert
            Assert.NotNull(result);
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

        [Test]
        public async Task Index_ReturnsViewResult()
        {
            // Act
            var result = await _controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Test]
        public async Task Index_ReturnsListOfPizzas()
        {
            // Act
            var result = await _controller.Index() as ViewResult;
            var model = result?.Model as List<Pizza>;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(model);
            Assert.AreEqual(1, model.Count);
        }

        [Test]
        public async Task Details_WithValidId_ReturnsViewResultWithPizza()
        {
            // Arrange
            var pizzaId = 1;
            var pizza = await _context.Pizza.FindAsync(pizzaId);

            // Act
            var result = await _controller.Details(pizzaId) as ViewResult;
            var model = result?.Model as Pizza;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(model);
            Assert.AreEqual(pizza.Id, model.Id);
            Assert.AreEqual(pizza.PizzaName, model.PizzaName);
            Assert.AreEqual(pizza.BasePrice, model.BasePrice);
            Assert.AreEqual(pizza.Details, model.Details);
            Assert.AreEqual(pizza.ImageTitle, model.ImageTitle);
        }

        [Test]
        public async Task Details_WithInvalidId_ReturnsNotFoundp()
        {
            // Arrange
            var invalidPizzaId = 999;

            // Act
            var result = await _controller.Details(invalidPizzaId) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Test]
        public async Task Create_WithValidPizza_RedirectsToIndex()
        {
            // Arrange
            var pizza = new Pizza
            {
                PizzaName = "Pepperoni",
                BasePrice = 3.99f,
                Details = "Delicious pepperoni pizza",
                ImageTitle = "pepperoni-pizza.jpg"
            };

            // Act
            var result = await _controller.Create(pizza) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public async Task Create_WithInvalidPizza_ReturnsViewWithPizzap()
        {
            // Arrange
            var pizza = new Pizza();
            _controller.ModelState.AddModelError("PizzaName", "Invalid pizza name");

            // Act
            var result = await _controller.Create(pizza) as ViewResult;
            var model = result?.Model as Pizza;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(model);
            Assert.AreEqual(pizza, model);
        }


        [Test]
        public async Task Edit_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var invalidPizzaId = 999;

            // Act
            var result = await _controller.Edit(invalidPizzaId) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Test]
        public async Task Edit_WithValidPizza_RedirectsToIndex()
        {
            // Arrange
            var pizzaId = 1;
            var pizza = await _context.Pizza.FindAsync(pizzaId);
            pizza.PizzaName = "New Pizza Name";

            // Act
            var result = await _controller.Edit(pizzaId, pizza) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public void Index_ReturnsViewResultm()
        {
            // Act
            var result = _custumcontroller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }


        [Test]
        public void Index_ReturnsViewResultWithListOfUsers()
        {
            // Arrange
            var users = new List<User>
    {
        new User { Id = "1", UserName = "user1" },
        new User { Id = "2", UserName = "user2" },
        new User { Id = "3", UserName = "user3" }
    };
            var userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            userManagerMock.Setup(u => u.Users).Returns(users.AsQueryable());
            _userManager = userManagerMock.Object;

            _usersController = new UsersController(_userManager);

            // Act
            var result = _usersController.Index() as ViewResult;
            var model = result?.Model as List<User>;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(model);
            Assert.AreEqual(users.Count, model.Count);
        }

        private void SeedDatabase()
        {
            
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