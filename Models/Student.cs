using System.ComponentModel.DataAnnotations;

namespace StudentAPI.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters")]
        public string Name { get; set; }

        [Range(0.0, 10.0,ErrorMessage = "CGPA must be between 0 and 10")]
        public double CGPA { get; set; }

        [Range(16, 100,
            ErrorMessage = "Age must be between 15 and 100")]
        public int Age { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int IsActive { get; set; }
    }
}
