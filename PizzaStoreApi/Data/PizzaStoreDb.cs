using Microsoft.EntityFrameworkCore;
using PizzaStoreApi.Models;

namespace PizzaStoreApi.Data
{
    public class PizzaStoreDb : DbContext
    {
        public PizzaStoreDb(DbContextOptions options) : base(options) { }

        public DbSet<Pizza> Pizzas { get; set; } = null!;
    }
}
