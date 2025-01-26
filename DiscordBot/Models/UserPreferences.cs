using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscordBot.Models
{
    public class UserPreferences
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public ulong DiscordUserId { get; set; }

        public Dictionary<string, string> Preferences { get; set; } = new Dictionary<string, string>
        {
            { "language", "en_US" }
        };
    }
}
