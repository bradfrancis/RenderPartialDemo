using System.Data.Entity;

namespace RenderPartialDemo.Models
{
    public class GreetingsDbContext : DbContext
    {
        public GreetingsDbContext()
            : base("GreetingsDb")
        {
        }

        public DbSet<Greeting> Greetings { get; set; }
    }
}