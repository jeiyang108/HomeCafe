using Microsoft.EntityFrameworkCore;
using webapi.Models.DomainModels;
using Type = webapi.Models.DomainModels.Type;

namespace webapi.Data
{
    public class CafeDbContext : DbContext
    {
        public CafeDbContext(DbContextOptions options) : base(options) 
        {
        }

        public DbSet<Drink> Drinks { get; set; }
        
        public DbSet<OrderedDrinkIngredient> OrderedDrinkIngredients { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<DrinkType> DrinkTypes { get; set; }
        public DbSet<DrinkIngredient> DrinkIngredients { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Photo> Photos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define composite keys in the entity models
            modelBuilder.Entity<DrinkIngredient>()
                .HasKey(di => new { di.DrinkId, di.IngredientId });
            modelBuilder.Entity<DrinkType>()
                .HasKey(di => new { di.DrinkId, di.TypeId });
            modelBuilder.Entity<OrderedDrinkIngredient>()
                .HasKey(di => new { di.OrderedDrinkId, di.IngredientId });

            // Define and insert sample data for the entities.
            Seed(modelBuilder);
        }

        private void Seed(ModelBuilder modelBuilder)
        {
            var pump = new Unit { Id = 1, Name = "pump(s)" };
            var tsp = new Unit { Id = 2, Name = "tsp(s)" };
            var shot = new Unit { Id = 3, Name = "shot(s)" };
            var other = new Unit { Id = 4, Name = "other" };
            var undefined = new Unit { Id = 5, Name = "undefined" };
            var baseIng = new Unit { Id = 6, Name = "base" };
            
            modelBuilder.Entity<Unit>().HasData(
                pump, tsp, shot, other, undefined, baseIng
            );

            var coffee = new Type { Id = 1, Name = "Coffee" };
            var tea = new Type { Id = 2, Name = "Tea" };
            var decaf = new Type { Id = 3, Name = "Decaf" };
            var hot = new Type { Id = 4, Name = "Hot" };
            var cold = new Type { Id = 5, Name = "Cold" };
			modelBuilder.Entity<Type>().HasData(
                coffee, tea, decaf, hot, cold
            );

            modelBuilder.Entity<Status>().HasData(
                new Status { Id = 1, Name = "New Order" },
                new Status { Id = 2, Name = "In Progress" },
                new Status { Id = 3, Name = "Completed" },
                new Status { Id = 4, Name = "Cancelled" }
            );

            byte[] imageData = File.ReadAllBytes("Images/CoffeeImage.jpg");
            var samplePhoto1 = new Photo { Id = 1, Image = imageData };
			var samplePhoto2 = new Photo { Id = 2, Image = imageData };
			modelBuilder.Entity<Photo>().HasData(
                samplePhoto1, samplePhoto2
			);

            var vanilla = new Ingredient { Id = 1, Name = "Sugar-free vanilla", UnitId = pump.Id };
            var hazelnut = new Ingredient { Id = 2, Name = "Sugar-free hazelnut", UnitId = pump.Id };
            var cocoa = new Ingredient { Id = 3, Name = "Chocolate (Cocoa powder)", UnitId = tsp.Id };
            var sugar = new Ingredient { Id = 4, Name = "Sugar (white)", UnitId = tsp.Id };
            var cinnamon = new Ingredient { Id = 5, Name = "Cinnamon powder", UnitId = tsp.Id };
            var milk2 = new Ingredient { Id = 6, Name = "2% milk", UnitId = baseIng.Id };
            var milk3 = new Ingredient { Id = 7, Name = "3% milk", UnitId = baseIng.Id };
            var ice = new Ingredient { Id = 8, Name = "Ice", UnitId = other.Id };
            var espresso = new Ingredient { Id = 9, Name = "Espresso shot", UnitId = shot.Id };
            var hotWater = new Ingredient { Id = 10, Name = "Hot water", UnitId = baseIng.Id };
            var water = new Ingredient { Id = 11, Name = "Water", UnitId = baseIng.Id };
            modelBuilder.Entity<Ingredient>().HasData(
               vanilla, hazelnut, cocoa, sugar, cinnamon, milk2, milk3, ice, espresso, hotWater, water
            );


            var latte = new Drink { Id = 1, Name = "Latte", Description = "Cafe latte made with espresso and steamed milk", DateCreated = new DateTime(), IsActive = true, PhotoId = samplePhoto1.Id };
            var icedAmeriano = new Drink { Id = 2, Name = "Iced Americano", Description = "Iced americano made with espresso shots", DateCreated = new DateTime(), IsActive = true, PhotoId = samplePhoto2.Id };
            modelBuilder.Entity<Drink>().HasData(
               latte, icedAmeriano
            );

            modelBuilder.Entity<DrinkIngredient>().HasData(
               new DrinkIngredient { DrinkId = latte.Id, IngredientId = milk2.Id, Amount = 0 },
               new DrinkIngredient { DrinkId = latte.Id, IngredientId = espresso.Id, Amount = 2 },
               new DrinkIngredient { DrinkId = icedAmeriano.Id, IngredientId = ice.Id, Amount = 0 },
               new DrinkIngredient { DrinkId = icedAmeriano.Id, IngredientId = espresso.Id, Amount = 2 },
               new DrinkIngredient { DrinkId = icedAmeriano.Id, IngredientId = water.Id, Amount = 0 }
            );


            var encryptedPassword = PasswordHelper.HashPassword("password");

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "Jei Yang", Password = encryptedPassword, DateCreated = new DateTime(), Email = "octoberjei@gmail.com", EmailConfirmed = true, IsAdmin = true },
                new User { Id = 2, Name = "Jeremy Wildsmith", Password = encryptedPassword, DateCreated = new DateTime(), Email = "jeremyawildsmith@gmail.com", EmailConfirmed = true, IsAdmin = false }
            );

            modelBuilder.Entity<DrinkType>().HasData(
                new DrinkType { DrinkId = latte.Id, TypeId = coffee.Id },
                new DrinkType { DrinkId = latte.Id, TypeId = hot.Id },
				new DrinkType { DrinkId = icedAmeriano.Id, TypeId = coffee.Id },
				new DrinkType { DrinkId = icedAmeriano.Id, TypeId = cold.Id }
			);
            
        }
    }
}
