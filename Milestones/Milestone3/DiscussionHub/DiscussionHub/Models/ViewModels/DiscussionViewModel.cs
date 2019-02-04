namespace DiscussionHub.Models.ViewModels
{
    public class DiscussionViewModel
    {
        public Discussion DiscussionDetails { get; set; }

        public string Username { get; set; }

        public bool IsAuthor { get; set; } = false;
    }
}