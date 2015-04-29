using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class PlayerViewModel
    {
        [Required(ErrorMessage = "First name is required")]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Display name is required")]
        [Display(Name = "Display name")]
        public string DisplayName { get; set; }
    }
}