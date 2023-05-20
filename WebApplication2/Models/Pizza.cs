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
        public static float getFinale(bool tomatoSauce, bool cheese, bool pepperoni, bool mushroom, bool tuna, bool pineapple, bool ham, bool beef)
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

                if (tomatoSauce)
                {
                    final += TomatoSauce;
                }
                else if(cheese)
                {
                    final += Cheese;
                }
                else if(pepperoni)
                {
                    final += Peperoni;
                }
                else if(mushroom)
                {
                    final += Mushroom;
                }
                else if(tuna)
                {
                    final += Tuna;
                }
                else if(pineapple)
                {
                    final += Pineapple;
                }
                else if(ham)
                {
                    final += Ham;
                }
                else if(beef)
                {
                    final += Beef;
                }

            return final;
        }
    }
}
