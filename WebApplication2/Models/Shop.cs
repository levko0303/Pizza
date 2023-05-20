using Microsoft.AspNetCore.Identity;

namespace WebApplication2.Models
{
    public class Shop
    {
        public int Id { get; set; }

        public virtual IdentityUser User { get; set; }

        public virtual Pizza Pizza { get; set; }
    }
}
