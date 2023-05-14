using WebApplication2.Data;
using WebApplication2.Models;

public static class ApplicationDbContextExtensions
{
    public static void SeedData(this ApplicationDbContext context)
    {
        if (context.Pizza.Any())
        {
            // База даних уже містить деякі піци, тому додавати їх знову не потрібно.
            return;
        }

        context.Pizza.Add(new Pizza() { ImageTitle = "Margerita", PizzaName = "Margerita", BasePrice = 2, TomatoSauce = true, Cheese = true, FinalPrice = 4, Details = "Неаполітанський соус, сир моцарела." });
        context.Pizza.Add(new Pizza() { ImageTitle = "Bolognese", PizzaName = "Bolognese", BasePrice = 2, TomatoSauce = true, Cheese = true, Beef = true, FinalPrice = 5, Details = "Неаполітанський соус, сир моцарела." });
        context.Pizza.Add(new Pizza() { ImageTitle = "Hawaiian", PizzaName = "Hawaiian", BasePrice = 2, TomatoSauce = true, Cheese = true, Ham = true, Pineapple = true, FinalPrice = 15, Details = "Неаполітанський соус, сир моцарела." });
        context.Pizza.Add(new Pizza() { ImageTitle = "Carbonara", PizzaName = "Carbonara", BasePrice = 2, TomatoSauce = true, Cheese = true, Ham = true, Mushroom = true, FinalPrice = 6, Details = "Неаполітанський соус, сир моцарела." });
        context.Pizza.Add(new Pizza() { ImageTitle = "Meatfeast", PizzaName = "Meatfeast", BasePrice = 2, TomatoSauce = true, Cheese = true, Ham = true, Beef = true, FinalPrice = 6, Details = "Неаполітанський соус, сир моцарела." });
        context.Pizza.Add(new Pizza() { ImageTitle = "Mushroom", PizzaName = "Mushroom", BasePrice = 2, TomatoSauce = true, Cheese = true, Mushroom = true, FinalPrice = 5 , Details = "Неаполітанський соус, сир моцарела." });
        context.Pizza.Add(new Pizza() { ImageTitle = "Pepperoni", PizzaName = "Pepperoni", BasePrice = 2, TomatoSauce = true, Cheese = true, Peperoni = true, FinalPrice = 5 , Details = "Неаполітанський соус, сир моцарела." });
        context.Pizza.Add(new Pizza() { ImageTitle = "Seafood", PizzaName = "Seafood", BasePrice = 2, TomatoSauce = true, Cheese = true, Tuna = true, FinalPrice = 5, Details = "Неаполітанський соус, сир моцарела." });
        context.Pizza.Add(new Pizza() { ImageTitle = "Vegetarian", PizzaName = "Vegetarian", BasePrice = 2, TomatoSauce = true, Cheese = true, Mushroom = true, Pineapple = true, FinalPrice = 12, Details = "Неаполітанський соус, сир моцарела." });
        context.SaveChanges();
    }
}