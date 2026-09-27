using Microsoft.AspNetCore.Mvc;
using SubjectAPI.Models;
using SubjectAPI.Repository;

namespace SubjectAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubjectController : ControllerBase
    {
        private readonly SubjectRepository _repository;

        // Constructor
        public SubjectController(SubjectRepository repository)
        {
            _repository = repository;
        }


        // GET: api/Subject
        [HttpGet]
        public async Task<IActionResult> GetSubjects()
        {
            try
            {
                var subjects = await _repository.GetAll();

                return Ok(subjects);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while getting subjects.",
                    error = ex.Message
                });
            }
        }


        // GET: api/Subject/11
        [HttpGet("{subjectCode}")]
        public async Task<IActionResult> GetSubjectByCode(int subjectCode)
        {
            try
            {
                var subject =
                    await _repository.GetSubjectByCode(subjectCode);

                if (subject == null)
                {
                    return NotFound("Subject not found.");
                }

                return Ok(subject);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while getting the subject.",
                    error = ex.Message
                });
            }
        }


        // POST: api/Subject
        [HttpPost]
        public async Task<IActionResult> AddSubject(Subject subject)
        {
            try
            {
                var newSubject =
                    await _repository.AddSubject(subject);

                return Ok(newSubject);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while adding the subject.",
                    error = ex.Message
                });
            }
        }


        // PUT: api/Subject/11
        [HttpPut("{subjectCode}")]
        public async Task<IActionResult> UpdateSubject(
            int subjectCode,
            Subject subject)
        {
            try
            {
                var updatedSubject =
                    await _repository.UpdateSubject(
                        subjectCode,
                        subject);

                if (updatedSubject == null)
                {
                    return NotFound("Subject not found.");
                }

                return Ok(updatedSubject);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while updating the subject.",
                    error = ex.Message
                });
            }
        }


        // DELETE: api/Subject/11
        [HttpDelete("{subjectCode}")]
        public async Task<IActionResult> DeleteSubject(int subjectCode)
        {
            try
            {
                var result =
                    await _repository.DeleteSubject(subjectCode);

                if (!result)
                {
                    return NotFound("Subject not found.");
                }

                return Ok("Subject deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while deleting the subject.",
                    error = ex.Message
                });
            }
        }
    }
}