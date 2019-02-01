namespace DiscussionHub.Models
{
    public class Discussion
    {

        public int DiscussionId { get; set; }

        public long VoteCount { get; set; }

        public long UpvoteCount { get; set; }

        public long DownvoteCount { get; set; }

        public long CommentCount { get; set; }

        public long TotalViews { get; set; }

        public int Rank { get; set; }

        public int UserId { get; set; }

        public string ArticleLink { get; set; }

        public string Title { get; set; }

        public string Contents { get; set; }

    }
}