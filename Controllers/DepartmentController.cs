using Microsoft.AspNetCore.Mvc;
using DepartmentAPI.Models;
using DepartmentAPI.Repository;

namespace DepartmentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly DepartmentRepository _repository;

        // Constructor
        public DepartmentController(DepartmentRepository repository)
        {
            _repository = repository;
        }


        // 1. GET - Get all departments
        [HttpGet]
        public async Task<IActionResult> GetDepartment()
        {
            try
            {
                var departments = await _repository.GetAll();

                return Ok(departments);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message = "An error occurred while getting departments.",
                        error = ex.Message
                    });
            }
        }


        // 2. GET BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            try
            {
                var department = await _repository.GetById(id);

                if (department == null)
                {
                    return NotFound("Department not found");
                }

                return Ok(department);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message = "An error occurred while getting the department.",
                        error = ex.Message
                    });
            }
        }


        // 3. POST - Add department
        [HttpPost]
        public async Task<IActionResult> AddDepartment(
            Department department)
        {
            try
            {
                var newDepartment =
                    await _repository.Add(department);

                return Ok(newDepartment);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message = "An error occurred while adding the department.",
                        error = ex.Message
                    });
            }
        }


        // 4. PUT - Update department
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(
            int id,
            Department department)
        {
            try
            {
                var updatedDepartment =
                    await _repository.Update(id, department);

                if (updatedDepartment == null)
                {
                    return NotFound("Department not found");
                }

                return Ok(updatedDepartment);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message = "An error occurred while updating the department.",
                        error = ex.Message
                    });
            }
        }


        // 5. DELETE - Delete department
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                var deleted =
                    await _repository.Delete(id);

                if (!deleted)
                {
                    return NotFound("Department not found");
                }

                return Ok("Department deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message = "An error occurred while deleting the department.",
                        error = ex.Message
                    });
            }
        }
    }
}