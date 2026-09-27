using Microsoft.AspNetCore.Mvc;
using AttendanceAPI.Models;
using AttendanceAPI.Repository;

namespace AttendanceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceController : ControllerBase
    {
        private readonly AttendanceRepository _repository;

        // Constructor
        public AttendanceController(AttendanceRepository repository)
        {
            _repository = repository;
        }


        // =========================================================
        // 1. GET ALL ATTENDANCE
        // GET: api/Attendance
        // =========================================================
        [HttpGet]
        public async Task<IActionResult> GetAttendance()
        {
            try
            {
                var attendance = await _repository.GetAll();

                return Ok(attendance);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message = "An error occurred while getting attendance records.",
                        error = ex.Message
                    });
            }
        }


        // =========================================================
        // 2. GET ATTENDANCE BY STUDENT ID
        // GET: api/Attendance/1
        // =========================================================
        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetByStudentId(int studentId)
        {
            try
            {
                if (studentId <= 0)
                {
                    return BadRequest(
                        "StudentID must be greater than 0.");
                }

                var attendance =
                    await _repository.GetByStudentId(studentId);

                if (attendance == null)
                {
                    return NotFound(
                        "Attendance record not found.");
                }

                return Ok(attendance);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message = "An error occurred while getting the attendance record.",
                        error = ex.Message
                    });
            }
        }


        // =========================================================
        // 3. ADD ATTENDANCE
        // POST: api/Attendance
        // =========================================================
        [HttpPost]
        public async Task<IActionResult> AddAttendance(
            [FromBody] Attendance attendance)
        {
            try
            {
                // Validate Status
                if (attendance.Status != "P" &&
                    attendance.Status != "A")
                {
                    return BadRequest(
                        "Status must be P (Present) or A (Absent).");
                }

                var newAttendance =
                    await _repository.Add(attendance);

                return Ok(newAttendance);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message = "An error occurred while adding attendance.",
                        error = ex.Message
                    });
            }
        }


        // =========================================================
        // 4. UPDATE ATTENDANCE
        // PUT: api/Attendance/1
        // =========================================================
        [HttpPut("{studentId}")]
        public async Task<IActionResult> UpdateAttendance(
            int studentId,
            [FromBody] Attendance attendance)
        {
            try
            {
                if (studentId <= 0)
                {
                    return BadRequest(
                        "StudentID must be greater than 0.");
                }

                // Validate Status
                if (attendance.Status != "P" &&
                    attendance.Status != "A")
                {
                    return BadRequest(
                        "Status must be P (Present) or A (Absent).");
                }

                var updatedAttendance =
                    await _repository.Update(
                        studentId,
                        attendance);

                if (updatedAttendance == null)
                {
                    return NotFound(
                        "Attendance record not found.");
                }

                return Ok(updatedAttendance);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message = "An error occurred while updating attendance.",
                        error = ex.Message
                    });
            }
        }


        // =========================================================
        // 5. DELETE ATTENDANCE
        // DELETE: api/Attendance/1
        // =========================================================
        [HttpDelete("{studentId}")]
        public async Task<IActionResult> DeleteAttendance(
            int studentId)
        {
            try
            {
                if (studentId <= 0)
                {
                    return BadRequest(
                        "StudentID must be greater than 0.");
                }

                var deleted =
                    await _repository.Delete(studentId);

                if (!deleted)
                {
                    return NotFound(
                        "Attendance record not found.");
                }

                return Ok(
                    "Attendance deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message = "An error occurred while deleting attendance.",
                        error = ex.Message
                    });
            }
        }
    }
}

