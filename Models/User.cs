using System.ComponentModel.DataAnnotations;

namespace UserAPI.Models
{
    public class User
    {
        public int UserID { get; set; }

        // Name validation
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "Username must be between 2 and 100 characters.")]
        public string Username { get; set; } = string.Empty;

        // Email validation
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        public string Email { get; set; } = string.Empty;

        // Password validation
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(50, MinimumLength = 6,
            ErrorMessage = "Password must be between 6 and 100 characters.")]
        public string Password { get; set; } = string.Empty;

        public string? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public int IsActive { get; set; }
    }
}
