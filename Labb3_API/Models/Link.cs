using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Labb3_API.Models
{
    public class Link
    {
        [Key]
        public int LinkId { get; set; }
        public string website { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        [JsonIgnore]
        public Interest interest { get; set; }
    }
}
