using System.ComponentModel.DataAnnotations;

namespace ProjectDataStructure.ViewModel
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
