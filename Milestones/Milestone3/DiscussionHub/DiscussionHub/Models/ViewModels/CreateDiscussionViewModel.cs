using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DiscussionHub.Models.ViewModels
{
    public class CreateDiscussionViewModel
    {
        public int UserId { get; set; }

        public DateTime DatePosted { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [DisplayName("News Article Link")]
        public string ArticleLink { get; set; }

        [Required]
        public string Contents { get; set; }
    }
}