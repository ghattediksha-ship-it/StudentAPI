using System.ComponentModel.DataAnnotations; 
namespace SubjectAPI.Models {
public class Subject { 
    public int SubjectCode { get; set; } 
    public string SubjectName { get; set; } = string.Empty; 

    [Range(1, 10, ErrorMessage = "Credits must be between 1 and 10.")] 
    public int Credits { get; set; } 
    public int FacultyID { get; set; } 
    public int DepartmentID { get; set; } 
    public string? CreatedBy { get; set; } 
    public DateTime CreatedOn { get; set; } 
    public string? ModifiedBy { get; set; } 
    public DateTime? ModifiedOn { get; set; } 
    public int IsActive { get; set; } 
    }
}