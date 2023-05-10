using Microsoft.AspNetCore.Identity;

namespace WebApplication2.Models
{
    public class Cart
    {
        public int Id { get; set; }

        public virtual IdentityUser User { get; set; }

        public virtual Pizza Pizza { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
