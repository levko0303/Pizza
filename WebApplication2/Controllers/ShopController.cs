using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

            var pizza = await _context.Pizza
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pizza == null)
            {
                return NotFound();
            }
            var curr_usr = _context.Users.FirstOrDefault(m => m.UserName == User.Identity.Name);
            var cart = await _context.Cart
                .FirstOrDefaultAsync(m => m.Pizza == pizza && m.User == curr_usr);
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
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
