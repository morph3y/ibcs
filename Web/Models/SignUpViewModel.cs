using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class SignUpViewModel
    {
        private const string EMAIL_PATTERN = "[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
        private const string PASSWORD_PATTERN = "(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{6,15})$";

        [Required(ErrorMessage="Required")]
        [RegularExpression(EMAIL_PATTERN, ErrorMessage="Invalid email")]
        [Display(Name = "Your email")]
        public string UserEmail { get; set; }

        [Required]
        [RegularExpression(PASSWORD_PATTERN, ErrorMessage = "Password must contain at least one digit and one uppercase letter")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string UserPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }

    }
}