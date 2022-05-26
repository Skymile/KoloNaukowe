
using Microsoft.EntityFrameworkCore;
// Lazy     Loading (Leniwe wczytywanie) 
// Explicit Loading
// Eager    Loading (Nadgorliwe wczytywanie)

public static class EntityFrameworkTest
{
    public static void Test()
    {
        var lazy = new Lazy<string>(() => "abc");
        string b = lazy.Value;

        Console.WriteLine("Hello, World!");

        using (var db = new DataContext())
        {
            for (int i = 0; i < 10; i++)
                db.Users?.Add(new User
                {
                    Login = "Abc" + i.ToString(),
                    Password = "bac" + i.ToString(),
                    Posts = new List<Post>
            {
                new Post { Message = "Message1" + i.ToString(), Title = "Title" },
                new Post { Message = "Message2" + i.ToString(), Title = "Title" },
                new Post { Message = "Message3" + i.ToString(), Title = "Title" }
            }
                });

            db.SaveChanges();
        }

        using (var db = new DataContext())
        {
            var users = db.Users?.ToList();
            var posts = db.Posts?.ToList();

            ;
        }
    }

    public class DataContext : DbContext
    {
        public DbSet<User>? Users { get; set; }
        public DbSet<Post>? Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
         //
         //   builder
         //       .Entity<User>()
         //       .HasMany(i => i.Posts)
         //       .WithOne(i => i.User);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) =>
            options.UseInMemoryDatabase("UserDatabase");
    }
}
