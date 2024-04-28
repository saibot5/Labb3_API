namespace Labb3_API.Models
{
    public class UserInterestViewModel
    {
        public string UserName { get; set; }

        public List<Interest> Interests { get; set; } = new List<Interest>();
    }
}
