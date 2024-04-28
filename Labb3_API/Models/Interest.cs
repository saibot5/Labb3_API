using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Labb3_API.Models
{
    public class Interest
    {
        public int InterestId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [JsonIgnore]
        public List<User> Users { get; set; } = new List<User>();
        public List<Link> Links { get; set; } = new List<Link>();
    }
}
