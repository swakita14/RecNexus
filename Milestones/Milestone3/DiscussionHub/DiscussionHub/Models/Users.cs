namespace DiscussionHub.Models
{
    public class Users
    {

        public int UserId { get; set; }

        public string Browser { get; set; }

        public string FName { get; set; }

        public string LName { get; set; }

        public string Email { get; set; }

        public string LoginPref { get; set; }

        public long VoteTotal { get; set; }

        public string About { get; set; }

        public string Pseudonym { get; set; }

    }
}