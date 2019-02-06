using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DiscussionHub.Models.ViewModels
{
    public class DiscussionHubUserViewModel
    {
        public int UserId { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string FName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string LName { get; set; }

        [Required]
        [DisplayName("Username")]
        public string LoginPref { get; set; }

        public string About { get; set; }
    }
}