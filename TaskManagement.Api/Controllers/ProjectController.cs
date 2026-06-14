using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Domain.Abstractions;
using TaskManagement.Domain.Commands;
using TaskManagement.Domain.Models;
using TaskManagement.Service.Services.Abstractions;

namespace TaskManagement.Api.Controllers
{
    [Route("api/projects")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectService _projectService;
        public ProjectController(IProjectRepository projectRepository, IProjectService projectService)
        {
            _projectRepository = projectRepository;
            _projectService = projectService;
            
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<Project>> GetProject([FromRoute(Name ="id")] int projectId)
        {
           var project = await _projectRepository.GetByIdOrDefaultAsync(projectId);
            if (project is not null)
            {
                return Ok(project);
            }
            return NotFound();

        }

        [HttpGet]

        public async Task<ActionResult<List<Project>>> GetProjects()
        {
            var projects = await _projectRepository.ListAsync();
            return Ok(projects);
        }
        [HttpPost]

        public async Task<ActionResult<int>> RegisterProject([FromBody] RegisterProjectCommand command)
        {
            
           var projectId = await _projectService.ExecuteAsync(command);
            return Ok(projectId);

        }
        [HttpPost("open")]

        public async Task<ActionResult> OpenProject([FromBody] OpenProjectCommand command)
        {
             await _projectService.ExecuteAsync(command);
            return Ok();
        }

        [HttpPost("close")]
        public async Task<ActionResult> CloseProject([FromBody] CloseProjectCommand command)
        {
            await _projectService.ExecuteAsync(command);
            return Ok();
        }

        [HttpPost("suspend")]
        public async Task<ActionResult> SuspendProject([FromBody] SuspendProjectCommand command)
        {
            await _projectService.ExecuteAsync(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProject([FromRoute] int id)
        {
            await _projectRepository.DeleteAsync(id);
            return Ok("was deleted succesfully");
        }
    }
}
