using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;
using DiscordBot.Models;


namespace DiscordBot.Data
{
    public class BotDBContext : DbContext
    {
        public BotDBContext(DbContextOptions<BotDBContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserPreferences>().ToCollection("userprefs");
        }

        public DbSet<UserPreferences> UserPreferences { get; set; }
    }
}
