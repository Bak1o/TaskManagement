using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManagement.Domain.Abstractions;
using TaskManagement.Domain.Commands;
using TaskManagement.Domain.Models;
using TaskManagement.Domain.Queries;
using TaskManagement.Service.DataTransferObjects;
using TaskManagement.Service.Services.Abstractions;

namespace TaskManagement.Api.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class DomainTaskController : ControllerBase
    {
        private readonly IDomainTaskRepository _repository;
        private readonly IDomainTaskService _service;
        public DomainTaskController(IDomainTaskRepository repository, IDomainTaskService service)
        {
            _repository = repository;
            _service = service;
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<TaskDto>> GetTask([FromRoute(Name = "id")] int taskId)
        {
            var taskDto = await _repository.GetByIdDtoAsync(taskId);
            if (taskDto is not null)
            {
                return Ok(taskDto);
            }
            return NotFound();
        }

        [HttpGet("debug-token")]
        public IActionResult DebugToken()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null || !identity.IsAuthenticated)
            {
                return Unauthorized("Token is missing or invalid");
            }

            var claims = identity.Claims.Select(c => new { c.Type, c.Value }).ToList();
            return Ok(claims);
        }


        [Authorize(AuthenticationSchemes = "Bearer", Policy = "ManagerPolicy")]
        [HttpGet]

        public async Task<ActionResult<List<TaskDto>>> GetTasks([FromQuery]TaskQueryFilter filter)
        {
            var tasks = await _repository.ListAsync(filter);
            return Ok(tasks);
        }

        //[Authorize(Policy ="ManagerPolicy")]
        [HttpGet("{id}/with-assigned-users")]

        public async Task<ActionResult<TaskWithUsersDto>> GetTasksWithUsers([FromRoute(Name = "id")] int id)
        {
            

            return Ok(await _repository.GetByIdWithUsersAsync(id));

        }

        [HttpPut]

        public async Task<ActionResult<DomainTask>> UpdateTask([FromBody] UpdateTaskCommand command)
        {
          await _service.UpdateAsync(command);
            return Ok();
        }

        [HttpPut("close")]
        public async Task<ActionResult> CloseTask([FromBody] CloseTaskCommand command)
        {
            await _service.CloseAsync(command);
            return Ok();
        }
       
        
        [HttpPut("assign-user")]

        public async Task<ActionResult> AssignUser([FromBody] AssignUserCommands command)
        {
           await _service.AssignUserAsync(command.ApplicationUserEmail,command.TaskId);
            return Ok();
        }

        [HttpPut("remove-assigned-user")]

        public async Task<ActionResult> RemoveAssignedUser([FromBody] AssignUserCommands command)
        {
            await _service.RemoveAssignedUserAsync(command.ApplicationUserEmail, command.TaskId);
            return Ok();
        }

        [HttpPost]

        public async Task<ActionResult<DomainTask>> CreateTask([FromBody] OpenTaskCommand command)
        {

            var id = await _service.OpenAsync(command);
            return Ok(id);
        }

        [HttpDelete("{id}")]
        
        public async Task<ActionResult> DeleteTask([FromRoute] int id)
        {
            await _repository.DeleteAsync(id);
            return Ok("was deleted succesfully");
        }

    }

    }

