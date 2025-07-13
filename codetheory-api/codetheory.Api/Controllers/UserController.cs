using codetheory.BL.DTOs;
using codetheory.BL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace codetheory.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult<IEnumerable<UserDto>> GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "admin")]
        public ActionResult<UserDto> GetUser(int id)
        {
            try
            {
                var user = _userService.GetUserById(id);
                return Ok(user);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult AddUser([FromBody] CreateUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _userService.AddUser(userDto);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult UpdateUser(int id, [FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _userService.UpdateUser(id, userDto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                _userService.DeleteUser(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("role/{roleId}")]
        [Authorize(Roles = "admin")]
        public ActionResult<IEnumerable<UserDto>> GetUsersByRoleId(int roleId)
        {
            var users = _userService.GetUsersByRoleId(roleId);
            return Ok(users);
        }

        [HttpGet("by-username/{username}")]
        [Authorize]
        public ActionResult<UserDto> GetUserByUsername(string username)
        {
            try
            {
                var user = _userService.GetUserByUsername(username);
                return Ok(user);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("students/progress")]
        [Authorize(Roles = "teacher")]
        public ActionResult<IEnumerable<StudentWithProgressDto>> GetStudentsWithProgress()
        {
            var result = _userService.GetStudentsWithProgress();
            return Ok(result);
        }

        [HttpGet("students/progress/search")]
        [Authorize(Roles = "teacher")]
        public ActionResult<IEnumerable<StudentWithProgressDto>> SearchStudentsWithProgress([FromQuery] string term)
        {
            var result = _userService.SearchStudentsWithProgress(term);
            return Ok(result);
        }
    }
}
