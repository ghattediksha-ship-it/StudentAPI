using System.ComponentModel.DataAnnotations; 
namespace FacultyAPI.Models { 
    public class Faculty 
    { 
    public int FacultyId { get; set; } 
    public string FacultyName { get; set; } = string.Empty; 

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")] 
    [StringLength(150, ErrorMessage = "Email cannot exceed 150 characters.")] 
    public string Email { get; set; } = string.Empty; 
    
    [Range(0.01, 9999999999.99, ErrorMessage = "Salary must be greater than 0.")] 
    public decimal Salary { get; set; } 
    public string DepartmentName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Phone is required.")] 
    [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone number must contain exactly 10 digits.")] 
    public string Phone { get; set; } = string.Empty; 
    public string? CreatedBy { get; set; }
     public DateTime CreatedOn { get; set; } 
    public string? ModifiedBy { get; set; } 
    public DateTime? ModifiedOn { get; set; } 
    public int IsActive { get; set; } 
    }
}