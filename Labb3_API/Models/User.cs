using System.ComponentModel.DataAnnotations.Schema;

namespace Labb3_API.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string phonenumber { get; set; }

        public List<Interest> Interests { get; set; } = new List<Interest>();
    }
}
