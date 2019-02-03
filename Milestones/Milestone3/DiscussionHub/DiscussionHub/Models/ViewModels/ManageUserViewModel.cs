using System.Collections.Generic;

namespace DiscussionHub.Models.ViewModels
{
    public class ManageUserViewModel
    {
        public IndexViewModel Identity { get; set; }

        public DiscussionHubUser DiscussionHubUser { get; set; }

        public List<Discussion> Discussion { get; set; }
    }
}