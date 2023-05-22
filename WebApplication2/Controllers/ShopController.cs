using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;
namespace WebApplication2.Controllers
{
    [Authorize]
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShopController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return _context.Pizza != null ?
                          View(await _context.Pizza.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Pizza'  is null.");
        }
        public IActionResult MakeCustomPizza()
        {
            return View();
        }
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
                newcart.Quantity = 1;
                _context.Cart.Add(newcart);
            }
            else
            {
                cart.Quantity++;
            }
            await _context.SaveChangesAsync();

            await Task.Delay(1500); // Wait for 2 seconds

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AddCustomPizza(string pizzaName, bool tomatoSauce, bool cheese, bool peperoni, bool mushroom, bool tuna, bool pineapple, bool ham, bool beef)
        {
            // Create a new Pizza instance with the selected toppings
            Pizza customPizza = new Pizza
            {
                PizzaName = pizzaName,
                TomatoSauce = tomatoSauce,
                Cheese = cheese,
                Peperoni = peperoni,
                Mushroom = mushroom,
                Tuna = tuna,
                Pineapple = pineapple,
                Ham = ham,
                Beef = beef,
                Details = "Test",
                ImageTitle = pizzaName,
            };
            customPizza.FinalPrice = Pizza.getFinale(customPizza.TomatoSauce, customPizza.Cheese, customPizza.Peperoni, customPizza.Mushroom, customPizza.Tuna, customPizza.Pineapple, customPizza.Ham, customPizza.Beef) ;

            // Add the custom pizza to the database
            var curr_usr = _context.Users.FirstOrDefault(m => m.UserName == User.Identity.Name);
            var cart = await _context.Cart.FirstOrDefaultAsync(m => m.Pizza == customPizza && m.User == curr_usr);
            if (cart == null)
            {
                Cart newcart = new Cart();
                newcart.Pizza = customPizza;
                newcart.User = curr_usr;
                newcart.Quantity = 1;
                _context.Cart.Add(newcart);
            }
            else
            {
                cart.Quantity++;
            }
            await _context.SaveChangesAsync();

            await Task.Delay(1500); // Wait for 2 seconds

            // Redirect the user back to the main pizza page
            return RedirectToAction("Index", "Carts");
        }

    }
}
