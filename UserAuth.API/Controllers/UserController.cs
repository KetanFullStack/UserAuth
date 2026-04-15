using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserAuth.Application.Common;
using UserAuth.Application.Interfaces;
using UserAuth.Domain.Entities;

namespace UserAuth.API.Controllers
{
    /// <summary>
    /// Controller for managing user-related operations
    /// </summary>
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        public UserController(IUserService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves the list of all users
        /// </summary>
        /// <remarks>
        /// This endpoint requires a valid JWT token.
        /// 
        /// Example:
        /// GET /api/users
        /// 
        /// Header:
        /// Authorization: Bearer {token}
        /// </remarks>
        /// <returns>List of users</returns>
        /// <response code="200">Returns list of users</response>
        /// <response code="401">Unauthorized - Invalid or missing token</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _service.GetAllUsersAsync();

            return Ok(new ApiResponse<IEnumerable<User>>(users, "Users retrieved successfully"));
        }
    }
}
