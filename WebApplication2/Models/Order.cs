using Microsoft.AspNetCore.Identity;

namespace WebApplication2.Models
{
    public class Order
    {
        public int Id { get; set; }
        public virtual IdentityUser User { get; set; }

        public virtual List<PizzaToOrder> PizzaToOrder { get; set; }
    }
}
