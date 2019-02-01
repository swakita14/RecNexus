namespace DiscussionHub.Models
{
    public class Comment
    {

        public int CommentId { get; set; }

        public long VoteCount { get; set; }

        public long? UpvoteCount { get; set; }

        public long? DownvoteCount { get; set; }

        public string GifRequest { get; set; }

        public string ImageRequest { get; set; }

        public int UserId { get; set; }

        public int DiscussionId { get; set; }
    }
}