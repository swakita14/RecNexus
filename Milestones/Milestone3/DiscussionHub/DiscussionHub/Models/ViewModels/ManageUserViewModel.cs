namespace DiscussionHub.Models.ViewModels
{
    public class ManageUserViewModel
    {
        public IndexViewModel Identity { get; set; }

        public DiscussionHubUser DiscussionHubUser { get; set; }

        public Discussion Discussion { get; set; }
    }
}