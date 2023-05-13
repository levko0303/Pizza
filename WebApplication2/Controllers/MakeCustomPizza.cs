using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.Data;

namespace WebApplication2.Controllers
{
    public class MakeCustomPizza : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
