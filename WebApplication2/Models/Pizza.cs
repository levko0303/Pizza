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
            float final = 2;
            float TomatoSaucePrice = 2;
            float CheesePrice = 1;
            float PepperoniPrice = 0.5f;
            float MushroomPrice = 0.3f;
            float TunaPrice = 12;
            float PineapplePrice = 9;
            float HamPrice = 5;
            float BeefPrice = 1.5f;


            if (tomatoSauce)
                final += TomatoSaucePrice;

            if (cheese)
                final += CheesePrice;

            if (pepperoni)
                final += PepperoniPrice;

            if (mushroom)
                final += MushroomPrice;

            if (tuna)
                final += TunaPrice;

            if (pineapple)
                final += PineapplePrice;

            if (ham)
                final += HamPrice;

            if (beef)
                final += BeefPrice;

            return final;
        }

    }
}
