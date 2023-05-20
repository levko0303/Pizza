using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Authorize]
    public class CartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int? id)
        {
            if (id == null || _context.Pizza == null)
            {
                return NotFound();
            }

            var pizza = await _context.Pizza.FirstOrDefaultAsync(m => m.Id == id);
            if (pizza == null)
            {
                return NotFound();
            }

            var curr_usr = _context.Users.FirstOrDefault(m => m.UserName == User.Identity.Name);
            var cart = await _context.Cart.FirstOrDefaultAsync(m => m.Pizza == pizza && m.User == curr_usr);
            if (cart == null)
            {
                Cart newcart = new Cart();
                newcart.Pizza = pizza;
                newcart.User = curr_usr;
                _context.Cart.Add(newcart);
            }
            await _context.SaveChangesAsync();

            await Task.Delay(1500); // Wait for 2 seconds

            return RedirectToAction(nameof(Index));
        }

        // GET: Carts
        public Task<IActionResult> Index()
        {
            var curr_usr = _context.Users.FirstOrDefault(m => m.UserName == User.Identity.Name);
            return Task.FromResult<IActionResult>(_context.Cart != null ?
                          View(_context.Cart.ToList().Where(m => m.User == curr_usr)):
                          Problem("Entity set 'ApplicationDbContext.Cart'  is null."));
        }

        public IActionResult Buy()
        {
            var curr_usr = _context.Users.FirstOrDefault(m => m.UserName == User.Identity.Name);
            var cart = _context.Cart.ToList().Where(m => m.User == curr_usr);
            var newOrder = new Order();
            newOrder.User = curr_usr;

            _context.Orders.Add(newOrder);
            _context.SaveChanges();

            foreach(var el in cart)
            {
                PizzaToOrder newPizzaToOrder = new PizzaToOrder();
                newPizzaToOrder.Pizza = el.Pizza;
                newPizzaToOrder.Order = newOrder;
                _context.PizzaToOrder.Add(newPizzaToOrder);
                _context.Cart.Remove(el);
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cart == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        public async Task<IActionResult> DeleteAll()
        {
            if (_context.Cart == null)
            {
                return NotFound();
            }

            var cartItems = await _context.Cart.ToListAsync();
            if (cartItems == null || cartItems.Count == 0)
            {
                return NotFound();
            }

            _context.Cart.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index"); // Redirect to the desired action after deleting all items
        }

        public async Task<IActionResult> AddCustomPizza(string pizzaName, bool tomatoSauce, bool cheese, bool pepperoni, bool mushroom, bool tuna, bool pineapple, bool ham, bool beef)
        {
            // Create a new Pizza instance with the selected toppings
            var customPizza = new Pizza
            {
                PizzaName = pizzaName,
                TomatoSauce = tomatoSauce,
                Cheese = cheese,
                Peperoni = pepperoni,
                Mushroom = mushroom,
                Tuna = tuna,
                Pineapple = pineapple,
                Ham = ham,
                Beef = beef,
                Details = "Test",
                ImageTitle = pizzaName,
                FinalPrice = Pizza.getFinale(tomatoSauce, cheese, pepperoni, mushroom, tuna, pineapple, ham, beef)
            };

            // Get the current user
            var currentUser = _context.Users.FirstOrDefault(m => m.UserName == User.Identity.Name);

            
                // Create a new Cart instance for the custom pizza
            var shop = new Shop
            {
                Pizza = customPizza,
                User = currentUser,
            };

            _context.Shop.Add(shop);
            await _context.SaveChangesAsync();

            await Task.Delay(1500); // Wait for 1.5 seconds

            return RedirectToAction("Index", "Carts");
        }

        public IActionResult MakeCustomPizza()
        {
            return View();
        }
        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int item_id)
        {
            if (_context.Shop == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Cart'  is null.");
            }
            var cart = await _context.Shop.FindAsync(item_id);
            if (cart != null)
            {
                _context.Shop.Remove(cart);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
