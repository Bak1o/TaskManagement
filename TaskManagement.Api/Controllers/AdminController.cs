using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Identity.DataTransferObjects;
using TaskManagement.Identity.Models;
using TaskManagement.Identity.Queries;
using TaskManagement.Identity.Services.Abstractions;

namespace TaskManagement.Api.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly IUserRepository _userRepository;
        public AdminController(IIdentityService identityService, IUserRepository userRepository)
        {
            _identityService = identityService;
            _userRepository = userRepository;
        }

        [HttpPost("all-users")]
        public async Task<ActionResult<ApplicationUserDto>> GetUser(string email)
        {
            return Ok(await _identityService.GetUserByEmailAsync(email));
        }

        [HttpGet]
        public async Task<ActionResult<List<ApplicationUserDto>>> GetAllUsers()
        {
            return Ok(await _identityService.GetAllUsersAsync());
        }

        [HttpGet("{role}-users")]
        public async Task<ActionResult<List<ApplicationUserDto>>> GetUsersByRole([FromRoute(Name = "role")] string role)
        {
            return Ok(await _userRepository.GetUsersByRoleAsync(role));
        }

        [HttpPost("give-admin")]
        public async Task<ActionResult> GiveAdmin(string email)
        {
            await _identityService.GiveAdminAsync(email);
            return Ok();
        }

        [HttpPut("remove-admin")]
        public async Task<ActionResult> RemoveAdmin(string email)
        {
            await _identityService.RemoveAdminAsync(email);
            return Ok();
        }

        [HttpPut("ban-user")]
        public async Task<ActionResult> BanUser(string email)
        {
            await _identityService.BanUserAsync(email);
            return Ok();
        }

        [HttpGet("users")]
        public async Task<ActionResult<List<ApplicationUserDto>>> GetUsers([FromQuery]UsersQueryFilter filter)
        {
           return Ok(await _identityService.GetUsersAsync(filter));
        }
    }
}
