namespace Labb3_API.Models
{
    public class InterestLinkViewModel
    {
        public int InterestId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        List<Link> Links { get; set; } = new List<Link>();
    }
}
