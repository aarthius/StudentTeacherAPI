using Microsoft.AspNetCore.Mvc;
using StudentTeacherAPI.DAL;
using StudentTeacherAPI.Helpers;
using StudentTeacherAPI.Models;

namespace StudentTeacherAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserDAL _userDAL;
        private readonly JwtHelper _jwtHelper;

        public AuthController(IConfiguration config, JwtHelper jwtHelper)
        {
            _userDAL = new UserDAL(config.GetConnectionString("DefaultConnection")!);
            _jwtHelper = jwtHelper;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            // Check if email already exists
            var existing = _userDAL.GetUserByEmail(user.Email);
            if (existing != null)
                return BadRequest("Email already registered.");

            // Hash the password before saving
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            _userDAL.RegisterUser(user);
            return Ok("Registration successful!");
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest user)
        {
            // Find user by email
            var existing = _userDAL.GetUserByEmail(user.Email);
            if (existing == null)
                return Unauthorized("Invalid email or password.");

            // Verify password
            bool isValid = BCrypt.Net.BCrypt.Verify(user.Password, existing.Password);
            if (!isValid)
                return Unauthorized("Invalid email or password.");

            // Generate JWT token with role
            var token = _jwtHelper.GenerateToken(existing.Email, existing.Designation);
            return Ok(new { token, role = existing.Designation, name = existing.Name });
        }
    }
}