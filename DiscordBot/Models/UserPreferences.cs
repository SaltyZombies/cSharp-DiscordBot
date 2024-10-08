using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace DiscordBot.Models
{
    public class UserPreferences
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("discorduserId")]
        public ulong DiscordUserId { get; set; }

        [BsonElement("preferences")]
        public Dictionary<string, string> Preferences { get; set; } = new Dictionary<string, string>
        {
            { "language", "en_US" }
        };
    }
}
