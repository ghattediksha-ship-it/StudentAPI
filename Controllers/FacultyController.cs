using FacultyAPI.Models;
using FacultyAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FacultyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacultyController : ControllerBase
    {
        private readonly FacultyRepository _repository;


        // =========================================================
        // CONSTRUCTOR
        // =========================================================
        public FacultyController(FacultyRepository repository)
        {
            _repository = repository;
        }


        // =========================================================
        // 1. GET ALL FACULTY
        // GET: api/Faculty
        // =========================================================
        [HttpGet]
        public async Task<IActionResult> GetFaculty()
        {
            try
            {
                var faculties = await _repository.GetAll();

                return Ok(faculties);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message = "An error occurred while getting faculties.",
                        error = ex.Message
                    });
            }
        }


        // =========================================================
        // 2. GET FACULTY BY ID
        // GET: api/Faculty/101
        // =========================================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFacultyById(int id)
        {
            try
            {
                var faculty = await _repository.GetById(id);

                if (faculty == null)
                {
                    return NotFound("Faculty not found.");
                }

                return Ok(faculty);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message = "An error occurred while getting faculty.",
                        error = ex.Message
                    });
            }
        }


        // =========================================================
        // 3. ADD FACULTY
        // POST: api/Faculty
        // =========================================================
        [HttpPost]
        public async Task<IActionResult> AddFaculty(Faculty faculty)
        {
            try
            {
                var newFaculty = await _repository.Add(faculty);

                if (newFaculty == null)
                {
                    return StatusCode(
                        500,
                        new
                        {
                            message = "Faculty could not be added."
                        });
                }

                return CreatedAtAction(
                    nameof(GetFacultyById),
                    new { id = newFaculty.FacultyId },
                    newFaculty);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message = "An error occurred while adding faculty.",
                        error = ex.Message
                    });
            }
        }


        // =========================================================
        // 4. UPDATE FACULTY
        // PUT: api/Faculty/101
        // =========================================================
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFaculty(
            int id,
            Faculty faculty)
        {
            try
            {
                var updatedFaculty =
                    await _repository.Update(id, faculty);

                if (updatedFaculty == null)
                {
                    return NotFound("Faculty not found.");
                }

                return Ok(updatedFaculty);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message = "An error occurred while updating faculty.",
                        error = ex.Message
                    });
            }
        }


        // =========================================================
        // 5. SOFT DELETE FACULTY
        // DELETE: api/Faculty/101
        // =========================================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFaculty(int id)
        {
            try
            {
                var deleted = await _repository.Delete(id);

                if (!deleted)
                {
                    return NotFound("Faculty not found or already deleted.");
                }

                return Ok("Faculty deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message = "An error occurred while deleting faculty.",
                        error = ex.Message
                    });
            }
        }
    }
}