namespace WebApplication2.Models
{
    public class Pizza
    {
        public int Id { get; set; }
        public string ImageTitle { get; set; }
        public string PizzaName { get; set; }
        public float BasePrice { get; set; } = 2;
        public bool TomatoSauce { get; set; }
        public bool Cheese { get; set; }
        public bool Peperoni { get; set; }
        public bool Mushroom { get; set; }
        public bool Tuna { get; set; }
        public bool Pineapple { get; set; }
        public bool Ham { get; set; }
        public bool Beef { get; set; }
        public float FinalPrice { get; set; }
        public string Details { get; set; }
        public static float getFinale(Pizza pizza)
        {
            float final = 0;
            float TomatoSauce = 2,
            Cheese = 1,
            Peperoni = 0.5f,
            Mushroom = 0.3f,
            Tuna = 12,
            Pineapple = 9,
            Ham = 5,
            Beef = 1.5f;

            if (pizza == null)
            {
                return final;
            }
            else
            {
                if (pizza.TomatoSauce)
                {
                    final += TomatoSauce;
                }
                if (pizza.Cheese)
                {
                    final += Cheese;
                }
                if (pizza.Peperoni)
                {
                    final += Peperoni;
                }
                if (pizza.Mushroom)
                {
                    final += Mushroom;
                }
                if (pizza.Tuna)
                {
                    final += Tuna;
                }
                if (pizza.Pineapple)
                {
                    final += Pineapple;
                }
                if (pizza.Ham)
                {
                    final += Ham;
                }
                if (pizza.Beef)
                {
                    final += Beef;
                }
            }

            return final;
        }
    }
}
