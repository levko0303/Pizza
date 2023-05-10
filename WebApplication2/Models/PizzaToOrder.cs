namespace WebApplication2.Models
{
    public class PizzaToOrder
    {
        public int Id { get; set; }

        public virtual Pizza Pizza { get; set; }

        public virtual Order Order { get; set; }

    }
}
