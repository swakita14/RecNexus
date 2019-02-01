namespace DiscussionHub.Models
{
    public class DiscussionHubUserViewModel
    {
        public IndexViewModel Identity { get; set; }

        public DiscussionHubUser DiscussionHubUser { get; set; }

        public Discussion Discussion { get; set; }
    }
}