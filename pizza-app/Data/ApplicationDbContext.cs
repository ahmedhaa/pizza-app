using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using pizza_app.Entities.UserModel;
using pizza_app.Entities.Pizzas;
using Microsoft.Extensions.Configuration;
using pizza_app.Enums;


namespace pizza_app.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Commande> Commandes { get; set; }
        public DbSet<CommandePizza> CommandePizzas { get; set; } // Table d'association Many-to-Many

        public DbSet<Ingredient> Ingredients { get; set; } // DbSet pour Ingredients
        public DbSet<PizzaIngredient> PizzaIngredients { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Commande>()
         .Property(c => c.Status)
         .HasConversion(
             v => v.ToString(),
             v => (CommandeStatus)Enum.Parse(typeof(CommandeStatus), v));


            // Configuration du champ Status dans Commande
            modelBuilder.Entity<Commande>()
           .Property(c => c.Status)
           .HasDefaultValue(CommandeStatus.EnCours);

            // Configuration de la relation Many-to-Many entre Pizza et Ingredient
            modelBuilder.Entity<PizzaIngredient>()
                .HasKey(pi => new { pi.PizzaId, pi.IngredientId });

            modelBuilder.Entity<PizzaIngredient>()
                .HasOne(pi => pi.Pizza)
                .WithMany(p => p.PizzaIngredients)
                .HasForeignKey(pi => pi.PizzaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PizzaIngredient>()
                .HasOne(pi => pi.Ingredient)
                .WithMany(i => i.PizzaIngredients)
                .HasForeignKey(pi => pi.IngredientId)
                .OnDelete(DeleteBehavior.Cascade);

            // Définition de la clé primaire composite pour la table de jointure
            modelBuilder.Entity<CommandePizza>()
                .HasKey(cp => new { cp.CommandeId, cp.PizzaId });

            // Relation Commande → CommandePizza
            modelBuilder.Entity<CommandePizza>()
                .HasOne(cp => cp.Commande)
                .WithMany(c => c.CommandePizzas)
                .HasForeignKey(cp => cp.CommandeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relation Pizza → CommandePizza
            modelBuilder.Entity<CommandePizza>()
                .HasOne(cp => cp.Pizza)
                .WithMany(p => p.CommandePizzas)
                .HasForeignKey(cp => cp.PizzaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
