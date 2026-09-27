using Microsoft.AspNetCore.Mvc;
using UserAPI.Models;
using UserAPI.Repository;

namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _repository;

        public UserController(UserRepository repository)
        {
            _repository = repository;
        }


        // =====================================================
        // GET: api/User
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _repository.GetAllUsers();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while getting users.",
                    error = ex.Message
                });
            }
        }


        // =====================================================
        // GET: api/User/1
        // =====================================================

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        message = "UserID must be greater than 0."
                    });
                }

                var user = await _repository.GetUserById(id);

                if (user == null)
                {
                    return NotFound(new
                    {
                        message = "User not found."
                    });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while getting the user.",
                    error = ex.Message
                });
            }
        }


        // =====================================================
        // POST: api/User
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var newUser = await _repository.AddUser(user);

                return Ok(newUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while adding the user.",
                    error = ex.Message
                });
            }
        }


        // =====================================================
        // PUT: api/User/1
        // =====================================================

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(
            int id,
            User user)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        message = "UserID must be greater than 0."
                    });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedUser =
                    await _repository.UpdateUser(id, user);

                if (updatedUser == null)
                {
                    return NotFound(new
                    {
                        message = "User not found."
                    });
                }

                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while updating the user.",
                    error = ex.Message
                });
            }
        }


        // =====================================================
        // DELETE: api/User/1
        // =====================================================

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        message = "UserID must be greater than 0."
                    });
                }

                var result =
                    await _repository.DeleteUser(id);

                if (!result)
                {
                    return NotFound(new
                    {
                        message = "User not found or already deleted."
                    });
                }

                return Ok(new
                {
                    message = "User deleted successfully."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while deleting the user.",
                    error = ex.Message
                });
            }
        }
    }
}
