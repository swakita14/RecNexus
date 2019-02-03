using System;
using System.ComponentModel.DataAnnotations;

namespace DiscussionHub.Models
{
    public class Discussion
    {
        
        public int DiscussionId { get; set; }

        [Display(Name = "Total Votes")]
        public long VoteCount { get; set; }

        [Display(Name = "Upvotes")]
        public long UpvoteCount { get; set; }

        [Display(Name = "Downvotes")]
        public long DownvoteCount { get; set; }

        [Display(Name = "Total Comments")]
        public long CommentCount { get; set; }

        [Display(Name = "Total Views")]
        public long TotalViews { get; set; }

       
        public int Rank { get; set; }

        public int UserId { get; set; }

        [Required]
        [Display(Name = "Article")]
        public string ArticleLink { get; set; }


        public string Title { get; set; }

        public string Contents { get; set; }

        [Display(Name = "Time of Post")]
        public DateTime PostTime { get; set; } 
    }
}