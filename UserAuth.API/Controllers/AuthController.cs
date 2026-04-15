using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserAuth.Application.Common;
using UserAuth.Application.DTOs;
using UserAuth.Application.Interfaces;

namespace UserAuth.API.Controllers
{
    /// <summary>
    /// Handles authentication operations such as signup, login, and password management.
    /// </summary>
    [ApiController]
    [Route("api/auth")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        public AuthController(IAuthService service)
        {
            _service = service;
        }

        /// <summary>
        /// Registers a new user in the system
        /// </summary>
        /// <remarks>
        /// Creates a new user account and returns JWT access token.
        /// 
        /// Sample Request:
        /// POST /api/auth/signup
        /// 
        /// {
        ///   "name": "Ketan S",
        ///   "email": "ketan@example.com",
        ///   "mobile": "123456789",
        ///   "password": "Password@123"
        /// }
        /// </remarks>
        /// <param name="dto">User registration details</param>
        /// <returns>JWT token</returns>
        /// <response code="200">User registered successfully</response>
        /// <response code="400">Invalid input or user already exists</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("signup")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<string>("Validation failed",
                    ModelState.Values.SelectMany(v => v.Errors)
                                     .Select(e => e.ErrorMessage)
                                     .ToList()));
            }

            var token = await _service.RegisterAsync(dto);

            return Ok(new ApiResponse<string>(token, "User registered successfully"));
        }

        /// <summary>
        /// Authenticates user and returns JWT token
        /// </summary>
        /// <remarks>
        /// Validates user credentials and returns access token.
        /// 
        /// Sample Request:
        /// POST /api/auth/login
        /// 
        /// {
        ///   "email": "ketan@example.com",
        ///   "password": "Password@123"
        /// }
        /// </remarks>
        /// <param name="dto">Login credentials</param>
        /// <returns>JWT token</returns>
        /// <response code="200">Login successful</response>
        /// <response code="401">Invalid credentials</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<string>("Validation failed",
                    ModelState.Values.SelectMany(v => v.Errors)
                                     .Select(e => e.ErrorMessage)
                                     .ToList()));

            var token = await _service.LoginAsync(dto);

            if (token == null)
                return Unauthorized(new ApiResponse<string>("Invalid email or password"));

            return Ok(new ApiResponse<string>(token, "Login successful"));
        }

        /// <summary>
        /// Changes the password of the authenticated user
        /// </summary>
        /// <remarks>
        /// Requires a valid JWT token.
        /// 
        /// Updates the password of the currently logged-in user.
        /// 
        /// Sample Request:
        /// PUT /api/auth/change-password
        /// 
        /// Header:
        /// Authorization: Bearer {token}
        /// 
        /// Body:
        /// {
        ///   "oldPassword": "OldPassword@123",
        ///   "newPassword": "NewPassword@123"
        /// }
        /// </remarks>
        /// <param name="dto">Password change request</param>
        /// <returns>Status of operation</returns>
        /// <response code="200">Password changed successfully</response>
        /// <response code="400">Invalid request or incorrect current password</response>
        /// <response code="401">Unauthorized - Invalid or missing token</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("change-password")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<string>("Validation failed",
                    ModelState.Values.SelectMany(v => v.Errors)
                                     .Select(e => e.ErrorMessage)
                                     .ToList()));

            // Get logged-in user email from JWT
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized(new ApiResponse<string>("Invalid token"));

            var result = await _service.ChangePasswordAsync(email, dto);

            if (!result)
                return BadRequest(new ApiResponse<string>("Invalid request or incorrect current password"));

            return Ok(new ApiResponse<string>("Password changed successfully"));
        }
    }
}
