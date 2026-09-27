using System.ComponentModel.DataAnnotations;

namespace AttendanceAPI.Models
{
    public class Attendance
    {
        [Required(ErrorMessage = "StudentID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "StudentID must be greater than 0.")]
        public int StudentID { get; set; }

        [Required(ErrorMessage = "SubjectID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "SubjectID must be greater than 0.")]
        public int SubjectID { get; set; }

        [Required(ErrorMessage = "Attendance date is required.")]
        public DateTime AttendanceDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int IsActive { get; set; }
    }
}