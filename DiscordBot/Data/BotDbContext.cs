using Microsoft.EntityFrameworkCore;
using DiscordBot.Models;

namespace DiscordBot.Data
{
    public class BotDbContext : DbContext
    {
        public BotDbContext(DbContextOptions<BotDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserPreferences>().ToTable("userprefs");
        }

        public DbSet<UserPreferences> UserPreferences { get; set; }
    }
}
