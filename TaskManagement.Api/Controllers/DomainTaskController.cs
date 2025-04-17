using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManagement.Domain.Abstractions;
using TaskManagement.Domain.Commands;
using TaskManagement.Domain.Models;
using TaskManagement.Domain.Models.Enums;
using TaskManagement.Domain.Queries;
using TaskManagement.Identity.Models;
using TaskManagement.Service.DataTransferObjects;
using TaskManagement.Service.Services.Abstractions;
using TaskManagement.SqlRepository.Database;
using TaskManagement.SqlRepository.DataTransferObjects;

namespace TaskManagement.Api.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class DomainTaskController : ControllerBase
    {
        private readonly IDomainTaskRepository _repository;
        private readonly IDomainTaskService _service;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _dbContext;
        public DomainTaskController(IDomainTaskRepository repository, IDomainTaskService service, UserManager<ApplicationUser> userManager,
            AppDbContext dbContext)
        {
            _repository = repository;
            _service = service;
            _userManager = userManager;
            _dbContext = dbContext;
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

        public async Task<ActionResult<DomainTask>> UpdateTask(UpdateTaskCommand command)
        {
          await _service.UpdateAsync(command);
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

        public async Task<ActionResult<DomainTask>> CreateTask(OpenTaskCommand command)
        {

            var id = await _service.OpenAsync(command);
            //// Step 1: Find Assigned Users using UserManager
            //var assignedUsers = new List<ApplicationUser>();
            //foreach (var email in command.AssignedUsersEmails)
            //{
            //    var user = await _userManager.FindByEmailAsync(email);
            //    if (user != null)
            //    {
            //        assignedUsers.Add(user);
            //    }
            //}

            //if (!assignedUsers.Any())
            //{
            //    return BadRequest("No valid users found for assignment.");
            //}

            //// Step 2: Find the Creator User using UserManager
            //var creatorUser = await _userManager.FindByEmailAsync(command.CreatedByUserMail);
            //if (creatorUser == null)
            //{
            //    return BadRequest("Invalid creator email.");
            //}

            //// Step 3: Create the DomainTask
            //var task = new DomainTask
            //{
            //    Title = command.Title,
            //    Description = command.Description,
            //    ProjectId = command.ProjectId,
            //    Priority = command.Priority,
            //    DeadLine = command.DeadLine,
            //    CreatedByUserId = creatorUser.Id,
            //    CreatedByUser = creatorUser,
            //    Status = Status.ToDo
            //};

            //// Step 4: Create TaskUser Entries
            //var taskUsers = assignedUsers.Select(user => new TaskUser
            //{
            //    DomainTask = task,
            //    DomainTaskId = task.Id,
            //    ApplicationUser = user,
            //    ApplicationUserId = user.Id
            //}).ToList();

            //// Step 5: Add to DB and Save Changes

            //await _service.OpenAsync(task);
            ////await _dbContext.DomainTasks.AddAsync(task);
            //await _dbContext.TaskUsers.AddRangeAsync(taskUsers);
            //await _dbContext.SaveChangesAsync();

            return Ok(id);
        }

        [HttpDelete]
        
        public async Task<ActionResult> DeleteTask(int id)
        {
            await _repository.DeleteAsync(id);
            return Ok("was deleted succesfully");
        }

    }

    }

