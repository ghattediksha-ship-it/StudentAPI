using System.ComponentModel.DataAnnotations;

namespace DepartmentAPI.Models
{
    public class Department
    {
        public int DepartmentID { get; set; }

        [Required(ErrorMessage = "DepartmentName is required.")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "DepartmentName must be between 2 and 100 characters.")]
        public string DepartmentName { get; set; } = string.Empty;
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int IsActive { get; set; }
    }
}