using Microsoft.AspNetCore.Mvc;
using StudentAPI.Models;
using StudentAPI.Repository;

namespace StudentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly StudentRepository _repository;

        // Constructor
        public StudentController(StudentRepository repository)
        {
            _repository = repository;
        }


        // GET: api/Student
        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            try
            {
                var students = await _repository.GetAll();

                return Ok(students);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while getting students.",
                    error = ex.Message
                });
            }
        }


        // GET: api/Student/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            try
            {
                // Validate ID
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        message = "Student ID must be greater than 0."
                    });
                }

                var student = await _repository.GetById(id);

                if (student == null)
                {
                    return NotFound("Student not found");
                }

                return Ok(student);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while getting the student.",
                    error = ex.Message
                });
            }
        }


        // POST: api/Student
        [HttpPost]
        public async Task<IActionResult> AddStudent(Student student)
        {
            try
            {
                // Validate Student model
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var newStudent = await _repository.Add(student);

                return Ok(newStudent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while adding the student.",
                    error = ex.Message
                });
            }
        }


        // PUT: api/Student/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(
            int id,
            Student student)
        {
            try
            {
                // Validate ID
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        message = "Student ID must be greater than 0."
                    });
                }

                // Validate Student model
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedStudent =
                    await _repository.Update(id, student);

                if (updatedStudent == null)
                {
                    return NotFound("Student not found");
                }

                return Ok(updatedStudent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while updating the student.",
                    error = ex.Message
                });
            }
        }


        // DELETE: api/Student/1
  [HttpDelete("{id}")]
public async Task<IActionResult> DeleteStudent(int id)
{
    try
    {
        if (id <= 0)
        {
            return BadRequest(new
            {
                message = "Student ID must be greater than 0."
            });
        }

        var deleted = await _repository.Delete(id);

        if (!deleted)
        {
            return NotFound("Student not found");
        }

        return Ok("Student deleted successfully");
    }
    catch (Exception ex)
    {
        return StatusCode(500, new
        {
            message = "An error occurred while deleting the student.",
            error = ex.Message
        });
    }
}
    }
}
